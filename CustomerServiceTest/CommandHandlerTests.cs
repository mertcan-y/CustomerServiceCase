using CustomerService.Commands;
using CustomerService.Database;
using CustomerService.Database.Models;
using CustomerService.Responses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace CustomerServiceTest
{
    public class CommandHandlerTests
    {
        private readonly Mock<IValidator<Customer>> _validator;
        private readonly CustomerDBContext _context;
        private readonly CommandHandler _handler;

        public CommandHandlerTests()
        {
            _validator = new Mock<IValidator<Customer>>();
            _context = new CustomerDBContext();
            _handler = new CommandHandler(_context, _validator.Object);
        }

        [Fact]
        public async Task AddCustomerCommandHandle_WrongEmailFormat_ReturnsValidationException()
        {
            _validator.Setup(x => x.Validate(It.IsAny<Customer>()))
                .Returns(new FluentValidation.Results.ValidationResult(new List<ValidationFailure>() { new ValidationFailure("Email", "wrong")}));

            var command = new AddCustomerCommand("Name", "Surname", "mail_email.com");

            await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, default));
        }

        [Fact]
        public async Task AddCustomerCommandHandle_ValidData_ReturnsSuccessResponse()
        {

            var command = new AddCustomerCommand("Name", "Surname", "mail@email.com");

            _validator.Setup(x => x.Validate(It.IsAny<Customer>()))
                         .Returns(new FluentValidation.Results.ValidationResult());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<DataResponse<string>>(result);
            Assert.Equal("Customer added successfully!", ((DataResponse<string>)result).Message);
        }

        [Fact]
        public async Task DeleteCustomerCommandHandle_CustomerNotExist_ReturnsException()
        {
            var command = new DeleteCustomerCommand("test_guid");

            await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteCustomerCommandHandle_ExistingCustomer_ReturnsSuccessResponse()
        {
            var command = new DeleteCustomerCommand("test_guid");

            var existingCustomer = new Customer { Id = "test_guid", Name = "Name", Surname = "Surname", Email = "mail@email.com" };
            _context.Customers.Add(existingCustomer);
            _context.SaveChanges();

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<SuccessResponse>(result);
            Assert.Equal("Customer deleted successfully!", ((SuccessResponse)result).Message);
        }
    }

}
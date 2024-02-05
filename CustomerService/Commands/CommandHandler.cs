using CustomerService.Database;
using CustomerService.Database.Models;
using CustomerService.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerService.Commands
{
    public class CommandHandler : IRequestHandler<AddCustomerCommand, Response>, IRequestHandler<DeleteCustomerCommand, Response>
    {
        private CustomerDBContext _context;
        private IValidator<Customer> _validator;

        public CommandHandler(CustomerDBContext context, IValidator<Customer> validator)
        {
            _context = context;
            _validator = validator;
        }

        
        public async Task<Response> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer customer = new Customer()
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
            };

            var validationResp = _validator.Validate(customer);
            if (!validationResp.IsValid)
            {
                throw new ValidationException(validationResp.Errors);
            }

            _context.Customers.Add(customer);
            _context.SaveChanges();
            return new DataResponse<string>(customer.Id, "Customer added successfully!");
        }

        public async Task<Response> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (customer == null) throw new Exception($"Customer with id:'{request.Id}' not found!");
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return new SuccessResponse("Customer deleted successfully!");
        }
    }
}

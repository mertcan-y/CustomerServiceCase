using CustomerService.Database;
using CustomerService.Database.Models;
using CustomerService.Queries;
using CustomerService.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CustomerServiceTest
{
    public class QueryHandlerTests
    {
        private readonly CustomerDBContext _context;
        private readonly QueryHandler _handler;

        public QueryHandlerTests()
        {
            _context = new CustomerDBContext();
            _handler = new QueryHandler(_context);
        }

        [Fact]
        public async Task GetCustomerQueryHandle_CustomerNotExist_ReturnsFailResponse()
        {
            var query = new GetCustomerQuery("test_guid");

            await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));

            CleanupCustomers();
        }

        [Fact]
        public async Task GetCustomerQueryHandle_ExistingCustomer_ReturnsDataResponse1()
        {
            var query = new GetCustomerQuery("test_guid");

            var existingCustomer = new Customer { Id = "test_guid", Name = "Name", Surname = "Surname", Email = "mail@email.com" };
            _context.Customers.Add(existingCustomer);
            _context.SaveChanges();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.IsType<DataResponse<Customer>>(result);
            Assert.Equal(existingCustomer, ((DataResponse<Customer>)result).Data);

            CleanupCustomers();
        }

        [Fact]
        public async Task ListCustomerQueryHandle_ReturnsDataResponseWithListOfCustomers()
        {
            var query = new ListCustomerQuery("");

            var customers = new List<Customer>
            {
                new Customer { Id = "test_guid1", Name = "Name1", Surname = "Surname1", Email = "mail1@email.com" },
                new Customer { Id = "test_guid2", Name = "Name2", Surname = "Surname2", Email = "mail2@email.com" },
            };

            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.IsType<DataResponse<List<Customer>>>(result);
            Assert.NotEmpty((result as DataResponse<List<Customer>>).Data);

            CleanupCustomers();
        }

        [Fact]
        public async Task ListCustomerQueryHandle_ReturnsFailResponse()
        {
            var query = new ListCustomerQuery("z");

            var customers = new List<Customer>
            {
                new Customer { Id = "test_guid1", Name = "Name1", Surname = "Surname1", Email = "mail1@email.com" },
                new Customer { Id = "test_guid2", Name = "Name2", Surname = "Surname2", Email = "mail2@email.com" },
            };

            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.IsType<DataResponse<List<Customer>>>(result);
            Assert.Empty((result as DataResponse<List<Customer>>).Data);

            CleanupCustomers();
        }

        private void CleanupCustomers()
        {
            _context.Customers.RemoveRange(_context.Customers.ToList());
            _context.SaveChanges();
        }
    }
}

using CustomerService.Database;
using CustomerService.Database.Models;
using CustomerService.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerService.Queries
{
    public class QueryHandler : IRequestHandler<GetCustomerQuery, Response>, IRequestHandler<ListCustomerQuery, Response>
    {
        private CustomerDBContext _context;
        public QueryHandler(CustomerDBContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (customer == null) throw new Exception($"Customer with id:'{request.Id}' not found!");
            return new DataResponse<Customer>(customer);
        }

        public async Task<Response> Handle(ListCustomerQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Customer, bool>> predicate = x => false;

            predicate = predicate.Or(x => x.Name.Contains(request.SearchWord));
            predicate = predicate.Or(x => x.Surname.Contains(request.SearchWord));
            predicate = predicate.Or(x => x.Email.Contains(request.SearchWord));

            var customers = await _context.Customers
                .Where(predicate)
                .ToListAsync();

            return new DataResponse<List<Customer>>(customers);
        }
    }
}

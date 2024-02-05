using CustomerService.Responses;
using MediatR;

namespace CustomerService.Queries
{
    public class GetCustomerQuery : IRequest<Response>
    {
        public GetCustomerQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

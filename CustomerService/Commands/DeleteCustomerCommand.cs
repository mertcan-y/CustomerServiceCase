using CustomerService.Responses;
using MediatR;

namespace CustomerService.Commands
{
    public class DeleteCustomerCommand : IRequest<Response>
    {
        public DeleteCustomerCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

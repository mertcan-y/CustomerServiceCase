using CustomerService.Responses;
using MediatR;

namespace CustomerService.Commands
{
    public class AddCustomerCommand : IRequest<Response>
    {
        public AddCustomerCommand(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}

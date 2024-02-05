using CustomerService.Responses;
using MediatR;

namespace CustomerService.Queries
{
    public class ListCustomerQuery : IRequest<Response>
    {
        public ListCustomerQuery(string searchWord)
        {
            SearchWord = searchWord;
        }

        public string SearchWord { get; set; }
    }
}

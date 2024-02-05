namespace CustomerService.Responses
{
    public class ErrorResponse : Response
    {
        public ErrorResponse(string message) : base(false, message)
        {

        }
    }
}

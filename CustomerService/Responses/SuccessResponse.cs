namespace CustomerService.Responses
{
    public class SuccessResponse : Response
    {
        public SuccessResponse() : base(true, "Success")
        {

        }
        public SuccessResponse(string message) : base(true, message)
        {

        }
    }
}

namespace CustomerService.Responses
{
    public class DataResponse<T> : Response
    {
        public DataResponse(T data, string message) : base(true, message)
        {
            Data = data;
        }

        public DataResponse(T data) : this(data, "Success")
        {
        }

        public T Data { get; set; }
    }
}

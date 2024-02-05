using CustomerService.Responses;
using MediatR.Pipeline;
using System.Text.Json;

namespace CustomerService
{
    public class ExceptionHandlingBehavior<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : notnull
    where TException : Exception
    where TResponse : notnull, Response
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse, TException>> logger;

        public ExceptionHandlingBehavior(
            ILogger<ExceptionHandlingBehavior<TRequest, TResponse, TException>> logger)
        {
            this.logger = logger;
        }

        public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {
            var error = CreateExceptionError(exception);

            logger.LogError(JsonSerializer.Serialize(error));

            state.SetHandled(error as TResponse);

            return Task.FromResult(error);
        }

        private static ErrorResponse CreateExceptionError(TException exception)
        {
            //var methodName = exception.TargetSite?.DeclaringType?.DeclaringType?.FullName;
            //var message = exception.Message;
            //var innerException = exception.InnerException?.Message;
            //var stackTrace = exception.StackTrace;

            //return new ErrorResponse(methodName, message, innerException, stackTrace);

            return new ErrorResponse(exception.Message);
        }
    }


}

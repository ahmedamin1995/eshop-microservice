using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;



namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomerExceptionHandler(ILogger<CustomerExceptionHandler> logger) : IExceptionHandler
    {
        public  async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An error occurred while processing the request. msg :{Message} - time {time}", exception.Message,DateTime.UtcNow);


            (string Details , string Title ,int StatusCode) details = exception switch
            {
                BadRequestException=> (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
                InternalServerException => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError),
                NotFoundException => (exception.Message, exception.GetType().Name, StatusCodes.Status404NotFound),
                ValidationException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
                _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError)

            };

            ProblemDetails problem =new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Details,
                Status = details.StatusCode,
                Type = exception.GetType().Name,
                Instance = httpContext.Request.Path
            };


            problem.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if(exception is ValidationException validationException)
            {
                problem.Extensions.Add("errors", validationException.Errors);
            }

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken: cancellationToken);
            httpContext.Response.StatusCode = details.StatusCode;

            return true;
        }
    }
}

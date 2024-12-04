using jwtAuth.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine("Global Exception Handler invoked ...");
        Console.WriteLine("exception => " + exception);

        var problemDetails = new ProblemDetails { Instance = httpContext.Request.Path };
        if (exception is ApiCustomException e)
        {
            httpContext.Response.StatusCode = (int)e.StatusCode;
            problemDetails.Title = e.Message;
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails.Title = exception.Message;
        }
        logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);
        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext
            .Response.WriteAsJsonAsync(problemDetails, cancellationToken)
            .ConfigureAwait(false);
        return true;
    }
}

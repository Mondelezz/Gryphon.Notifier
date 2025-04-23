using Application.Exceptions;
using System.Net;
using System.Text.Json;
using NullReferenceException = Application.Exceptions.NullReferenceException;

namespace API;

public class GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NullReferenceException ex)
        {
            await HandleExceptionAsync(HttpStatusCode.NotFound, context, ex);
        }
        catch (EntityNotFoundException ex)
        {
            await HandleExceptionAsync(HttpStatusCode.NotFound, context, ex);
        }
        catch (JwtTokenException ex)
        {
            await HandleExceptionAsync(HttpStatusCode.NotFound, context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(HttpStatusCode.InternalServerError, context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpStatusCode status, HttpContext context, Exception Ex)
    {
        logger.LogError("Exception occupied during executing request: {Path}\n{Ex}", context.Request.Path, Ex);

        Exception? exception = Ex;
        List<string> exeptionMessageList = [];

        while (exception is not null)
        {
            exeptionMessageList.Add(exception.Message);
            exception = exception.InnerException;
        }

        string exceptionResult = JsonSerializer.Serialize(new
        {
            path = new { context.Request.Path.Value, context.Request.Method },
            errorlist = exeptionMessageList
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(exceptionResult);
    }
}

using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var problemDetails = new ProblemDetails();

        switch (exception)
        {
            case ApplicationException:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Detail = exception.Message;
                problemDetails.Title = "Application Error";
                break;
            case KeyNotFoundException:
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                problemDetails.Detail = exception.Message;
                problemDetails.Title = "Not Found";
                break;
            case ValidationException ex:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new ValidationProblemDetails(ex.Errors)
                {
                    Detail = ex.Message
                };
                problemDetails.Extensions.Add("invalidParams", ex.Errors);
                problemDetails.Title = "Validation Error";
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Detail = $"{exception.Message} - {exception.StackTrace}";
                problemDetails.Title = "Server error";
                break;
        }
        _logger.LogError(exception, "Exception in request");
        var result = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(result);
    }
}
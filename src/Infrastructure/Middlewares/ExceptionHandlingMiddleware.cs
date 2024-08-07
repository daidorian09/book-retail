using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware
{
    private const string ContentType = "application/json";
    private const string ApplicationError = "Application Error";
    private const string NotFound = "Not Found";
    private const string InvalidParams = "invalidParams";
    private const string ValidationError = "Validation Error";
    private const string ServerError = "Server Error";
    private const string ExceptionInRequest = "Exception in request";

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
        context.Response.ContentType = Application.Constants.AppConstants.ContentType;
        var response = context.Response;

        var problemDetails = new ProblemDetails();

        switch (exception)
        {
            case ApplicationException:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Detail = exception.Message;
                problemDetails.Title = Application.Constants.AppConstants.ApplicationError;
                break;
            case KeyNotFoundException:
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                problemDetails.Detail = exception.Message;
                problemDetails.Title = Application.Constants.AppConstants.NotFound;
                break;
            case ValidationException ex:
                response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                problemDetails = new ValidationProblemDetails(ex.Errors)
                {
                    Detail = ex.Message
                };
                problemDetails.Extensions.Add(InvalidParams, ex.Errors);
                problemDetails.Title = Application.Constants.AppConstants.ValidationError;
                break;
            case CustomerExistsException:
                response.StatusCode = (int)HttpStatusCode.Conflict;
                problemDetails.Detail = exception.Message;
                problemDetails.Title = Application.Constants.AppConstants.NotFound;
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Detail = $"{exception.Message} - {exception.StackTrace}";
                problemDetails.Title = Application.Constants.AppConstants.ServerError;
                break;
        }
        _logger.LogError(exception, Application.Constants.AppConstants.ExceptionInRequest);

        var result = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(result);
    }
}
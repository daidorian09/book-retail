using Application.Constants;
using Application.Exceptions;
using Infrastructure.Extensions;
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

            if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await httpContext.WriteForbiddenResponseAsync();
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = AppConstants.ContentType;
        var response = context.Response;

        ProblemDetails problemDetails;
        HttpStatusCode statusCode;

        if (AppConstants.ExceptionToStatusCode.TryGetValue(exception.GetType(), out var mapping))
        {
            statusCode = mapping.StatusCode;
            problemDetails = CreateProblemDetails(exception, statusCode, mapping.Title);
        }
        else if (exception is ValidationException validationException)
        {
            statusCode = HttpStatusCode.UnprocessableEntity;
            problemDetails = CreateValidationProblemDetails(validationException);
        }
        else
        {
            statusCode = HttpStatusCode.InternalServerError;
            problemDetails = CreateProblemDetails(exception, statusCode, AppConstants.ServerError);
            problemDetails.Detail += $" - {exception.StackTrace}";
        }

        response.StatusCode = (int)statusCode;
        response.ContentType = "application/problem+json";
        _logger.LogError(exception, AppConstants.ExceptionInRequest);

        var result = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(result);
    }

    private static ProblemDetails CreateProblemDetails(Exception exception, HttpStatusCode statusCode, string title)
    {
        return new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = exception.Message
        };
    }

    private static ProblemDetails CreateValidationProblemDetails(ValidationException ex)
    {
        var problemDetails = new ValidationProblemDetails(ex.Errors)
        {
            Detail = ex.Message
        };
        problemDetails.Extensions.Add(AppConstants.InvalidParams, ex.Errors);
        problemDetails.Title = AppConstants.ValidationError;
        return problemDetails;
    }
}
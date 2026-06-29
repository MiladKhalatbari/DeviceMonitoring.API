using DeviceMonitoring.Services.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DeviceMonitoring.API.ExceptionHandling;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var isExpectedException = exception is 
            UserAlreadyExistsException or
            ResourceNotFoundException or
            UnauthorizedAccessException or
            ArgumentException;

        if (isExpectedException)
        {
            logger.LogWarning(
                "Request failed with {ExceptionType}. TraceId: {TraceId}",
                exception.GetType().Name,
                httpContext.TraceIdentifier);
        }
        else
        {
            logger.LogError(
                exception,
                "Unhandled exception occurred. TraceId: {TraceId}",
                httpContext.TraceIdentifier);
        }

        var (statusCode, title, detail) = exception switch
        {
            UserAlreadyExistsException => (
                StatusCodes.Status409Conflict,
                "Username already exists",
                "Choose a different username."),

            ResourceNotFoundException => (
                StatusCodes.Status404NotFound,
                "Resource not found",
                exception.Message),

            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Invalid credentials",
                "The username or password is incorrect."),

            ArgumentException => (
                StatusCodes.Status400BadRequest,
                "Invalid request",
                exception.Message),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "An unexpected error occurred.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        if (environment.IsDevelopment() &&
            statusCode == StatusCodes.Status500InternalServerError)
        {
            problemDetails.Extensions["debugMessage"] = exception.Message;
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            options: null,
            contentType: "application/problem+json",
            cancellationToken: cancellationToken);

        return true;
    }
}
using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using MyProject.Contracts.Exceptions;
using MyProject.Contracts.Localization;
using MyProject.Contracts.Response;

namespace MyProject.API.Middleware;

public class ExceptionHandler(
    RequestDelegate next,
    ILogger<ExceptionHandler> logger,
    IResourceLocalizer resourceLocalizer,
    IHostEnvironment hostEnvironment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception caught in global ExceptionHandler: {ExceptionType} - {Message}",
                ex.GetType().Name, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var (statusCode, message, errors) = exception switch
        {
            ValidationException validationException => HandleValidationException(validationException),
            AppException appException => (appException.StatusCode,
                hostEnvironment.IsDevelopment() ? appException.TechnicalMessage : appException.Message, null),
            SecurityTokenExpiredException => (StatusCodes.Status401Unauthorized, "The token is expired", null),
            SecurityTokenException => (StatusCodes.Status401Unauthorized, "Invalid token", null),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", null)
        };

        logger.LogInformation("Handling exception: {ExceptionType}, StatusCode: {StatusCode}, Message: {Message}",
            exception.GetType().Name, statusCode, message);
        logger.LogInformation(
            $"Error: ${exception!.InnerException!.Message}, tracing details: ${exception.InnerException.StackTrace}");

        var response = errors != null
            ? ApiResponse<object>.ErrorResponse(errors, (HttpStatusCode)statusCode)
            : ApiResponse<object>.ErrorResponse(message, (HttpStatusCode)statusCode);

        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private static (int statusCode, string message, List<string>? errors) HandleValidationException(
        ValidationException validationException)
    {
        var errors = validationException.Errors
            .Select(error => $"{error.PropertyName}: {error.ErrorMessage}")
            .ToList();

        var message = errors.Count == 1
            ? "Validation failed"
            : $"Validation failed with {errors.Count} errors";

        return (StatusCodes.Status400BadRequest, message, errors);
    }
}
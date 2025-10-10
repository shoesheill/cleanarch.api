using Microsoft.AspNetCore.Http;
using System;

namespace MyProject.Contracts.Exceptions;

public abstract class AppException(string message, Exception? innerException, int statusCode)
    : Exception(message, innerException)
{
    public int StatusCode { get; } = statusCode;
    public string TechnicalMessage { get; } = innerException?.Message ?? message;
}

// 400: Bad Request
public class BadRequestException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status400BadRequest);

// 401: Unauthorized
public class UnauthorizedException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status401Unauthorized);

// 403: Forbidden
public class ForbiddenException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status403Forbidden);

// 404: Not Found
public class NotFoundException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status404NotFound);

// 405: Method Not Allowed
public class MethodNotAllowedException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status405MethodNotAllowed);

// 409: Conflict
public class ConflictException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status409Conflict);

// 422: Unprocessable Entity
public class UnprocessableEntityException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status422UnprocessableEntity);

// 429: Too Many Requests
public class TooManyRequestsException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status429TooManyRequests);

// 500: Internal Server Error
public class InternalServerErrorException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status500InternalServerError);

// 503: Service Unavailable
public class ServiceUnavailableException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status503ServiceUnavailable);

public class AppInvalidOperationException(string message, Exception? exception = null)
    : AppException(message, exception, StatusCodes.Status500InternalServerError);

// 400: Bad Request - Unsupported Culture
public class UnsupportedCultureException(string culture, string[] supportedCultures, Exception? exception = null)
    : BadRequestException(
        $"Culture '{culture}' is not supported. Supported cultures are: {string.Join(", ", supportedCultures)}",
        exception);
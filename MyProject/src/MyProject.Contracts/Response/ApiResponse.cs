using System.Collections.Generic;
using System.Net;

namespace MyProject.Contracts.Response;

public class ApiResponse<T>
{
    public ApiResponse()
    {
        Errors = new List<string>();
    }

    public bool IsSuccess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T? data, string? message = default,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ApiResponse<T> { IsSuccess = true, StatusCode = statusCode, Data = data, Message = message };
    }

    public static ApiResponse<T> ErrorResponse(string errorMessage,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false, StatusCode = statusCode, Message = errorMessage,
            Errors = new List<string> { errorMessage }
        };
        ;
    }

    public static ApiResponse<T> ErrorResponse(List<string> errors,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ApiResponse<T>
            { IsSuccess = false, StatusCode = statusCode, Message = "One or more errors occurred", Errors = errors };
    }

    public static ApiResponse<T> SuccessResponseWithoutData(string message = null,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ApiResponse<T> { IsSuccess = true, StatusCode = statusCode, Message = message };
    }
}
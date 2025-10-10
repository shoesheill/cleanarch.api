using Microsoft.AspNetCore.Http;

namespace MyProject.Application.Abstractions;

public interface ICurrentRequestContext
{
    string Scheme { get; }
    string Host { get; }
    string Referer { get; }
    string Origin { get; }
    string BaseUrl { get; }
    string FrontendUrl { get; }

    string CallbackUrl(HttpContext httpContext, string actionName = "", string apiVersion = null);
    string GetActionUrl(HttpContext httpContext, string actionName = "", string apiVersion = null);
}
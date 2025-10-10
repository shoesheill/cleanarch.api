using Asp.Versioning;

namespace MyProject.Api.Extensions;

public static class ApiVersioningExtensions
{
    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(option =>
        {
            option.AssumeDefaultVersionWhenUnspecified =
                true;
            option.DefaultApiVersion = new ApiVersion(1, 0);
            option.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using MyProject.Api.Extensions;

namespace MyProject.API.Extensions;

public static class SwaggerDependenciesExtensions
{
    public static IServiceCollection AddSwaggerDependencies(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            c.SchemaFilter<UlidSchemaFilter>();
            foreach (var description in provider.ApiVersionDescriptions)
                c.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = "Asec API",
                    Version = description.ApiVersion.ToString()
                });
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Using the Authorization header with Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securitySchema);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securitySchema, ["Bearer"] }
            });
        });
        return services;
    }
}
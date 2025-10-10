using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Contracts.Exceptions;
using System.Linq;

namespace MyProject.Contracts.Localization;

public static class LocalizationExtensions
{
    public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
    {
        // Configure localization to use resources from Infrastructure project
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        // Register the resource localizer with our custom implementation
        services.AddTransient<IResourceLocalizer, ResourceLocalizer>();

        return services;
    }

    public static IApplicationBuilder UseLocalizationMiddleware(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "en", "ne", "ja" };

        // Add culture validation middleware before request localization
        app.Use(async (context, next) =>
        {
            var cultureQuery = context.Request.Query["culture"].ToString();
            if (!string.IsNullOrEmpty(cultureQuery) && !supportedCultures.Contains(cultureQuery))
                throw new UnsupportedCultureException(cultureQuery, supportedCultures);

            await next();
        });

        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture("en")
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        // Query string provider to support ?culture=fr
        localizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider
        {
            QueryStringKey = "culture",
            UIQueryStringKey = "ui-culture"
        });
        app.UseRequestLocalization(localizationOptions);

        return app;
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace MyProject.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Register FluentValidation
        // services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
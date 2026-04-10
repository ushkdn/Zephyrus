using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Zephyrus.Catalog.Application.Behaviors;

namespace Zephyrus.Catalog.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services
            .AddMediatr()
            .AddValidation()
            .AddPipelines();
    }

    private static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        return services
            .AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ICatalogApplicationAssemblyMarker).Assembly));
    }

    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        return services
            .AddValidatorsFromAssembly(typeof(ICatalogApplicationAssemblyMarker).Assembly);
    }

    private static IServiceCollection AddPipelines(this IServiceCollection services)
    {
        return services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}
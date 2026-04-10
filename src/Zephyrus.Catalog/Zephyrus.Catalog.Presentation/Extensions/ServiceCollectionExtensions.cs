using Microsoft.Extensions.DependencyInjection;

namespace Zephyrus.Catalog.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(typeof(ICatalogPresentationAssemblyMarker).Assembly);

        return services;
    }
}
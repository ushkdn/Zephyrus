using Microsoft.Extensions.DependencyInjection;

namespace Zephyrus.Procurement.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(typeof(IProcurementPresentationAssemblyMarker).Assembly);

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Zephyrus.Supplier.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(typeof(ISupplierPresentationAssemblyMarker).Assembly);

        return services;
    }
}
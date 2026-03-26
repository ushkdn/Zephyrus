using Microsoft.Extensions.DependencyInjection;

namespace Zephyrus.Identity.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(typeof(IIdentityPresentationAssemblyMarker).Assembly);

        return services;
    }
}
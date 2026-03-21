using Zephyrus.Identity.Application.Extensions;
using Zephyrus.Identity.Infrastructure.Extensions;
using Zephyrus.Identity.Presentation;
using Zephyrus.Identity.Presentation.Extensions;

namespace Zephyrus.Identity.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLayers(this IServiceCollection services)
    {
        services.AddControllers().AddApplicationPart(typeof(IIdentityPresentationAssemblyMarker).Assembly);

        return services
            .AddApplicationLayer()
            .AddInfrastructureLayer()
            .AddPresentationLayer();
    }
}
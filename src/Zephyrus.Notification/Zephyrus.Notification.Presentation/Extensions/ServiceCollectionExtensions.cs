using Microsoft.Extensions.DependencyInjection;

namespace Zephyrus.Notification.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(INotificationPresentationAssemblyMarker).Assembly);

        return services;
    }
}

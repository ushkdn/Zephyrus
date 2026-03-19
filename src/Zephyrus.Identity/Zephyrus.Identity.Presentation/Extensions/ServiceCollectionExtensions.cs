using Microsoft.Extensions.DependencyInjection;

namespace Zephyrus.Identity.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        return services;
    }
}
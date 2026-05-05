using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zephyrus.Identity.Application.Behaviors;
using Zephyrus.Identity.Application.Settings;
using Zephyrus.SharedKernel.Common.Extensions;

namespace Zephyrus.Identity.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var authSettings = configuration.GetSectionOrThrow<AuthSettings>(AuthSettings.SectionName);
        services.AddSingleton(authSettings);

        return services
            .AddMediatr()
            .AddValidation()
            .AddPipelines();
    }

    private static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        return services
            .AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(IIdentityApplicationAssemblyMarker).Assembly)
            );
    }

    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        return services
            .AddValidatorsFromAssembly(typeof(IIdentityApplicationAssemblyMarker).Assembly);
    }

    private static IServiceCollection AddPipelines(this IServiceCollection services)
    {
        return services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}
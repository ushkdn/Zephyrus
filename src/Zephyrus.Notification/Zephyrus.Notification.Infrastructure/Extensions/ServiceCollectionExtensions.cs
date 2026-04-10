using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zephyrus.Notification.Application.Interfaces;
using Zephyrus.Notification.Infrastructure.Messaging.Consumers;
using Zephyrus.Notification.Infrastructure.Persistence;
using Zephyrus.Notification.Infrastructure.Persistence.Repositories;
using Zephyrus.Notification.Infrastructure.Settings;
using Zephyrus.SharedKernel.Common.Extensions;

namespace Zephyrus.Notification.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NotificationDatabase")
            ?? throw new InvalidOperationException("Connection string 'NotificationDatabase' is not configured.");

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddMessaging(configuration);

        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSectionOrThrow<RabbitMqSettings>(RabbitMqSettings.SectionName);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<PurchaseRequestCreatedConsumer>();
            x.AddConsumer<PurchaseRequestApprovedConsumer>();
            x.AddConsumer<PurchaseRequestRejectedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, "/", h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

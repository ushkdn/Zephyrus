using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.Catalog.Infrastructure.Messaging.Consumers;
using Zephyrus.Catalog.Infrastructure.Persistence;
using Zephyrus.Catalog.Infrastructure.Persistence.Repositories;
using Zephyrus.Catalog.Infrastructure.Settings;
using Zephyrus.SharedKernel.Common.Extensions;

namespace Zephyrus.Catalog.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CatalogDatabase")
            ?? throw new InvalidOperationException("Connection string 'CatalogDatabase' is not configured.");

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddMessaging(configuration);

        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSectionOrThrow<RabbitMqSettings>(RabbitMqSettings.SectionName);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CheckProductExistsConsumer>();

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
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zephyrus.SharedKernel.Common.Extensions;
using Zephyrus.SharedKernel.Contracts.Catalog;
using Zephyrus.Supplier.Application.Interfaces;
using Zephyrus.Supplier.Infrastructure.Messaging;
using Zephyrus.Supplier.Infrastructure.Messaging.Consumers;
using Zephyrus.Supplier.Infrastructure.Persistence;
using Zephyrus.Supplier.Infrastructure.Persistence.Repositories;
using Zephyrus.Supplier.Infrastructure.Settings;

namespace Zephyrus.Supplier.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SupplierDatabase")
            ?? throw new InvalidOperationException("Connection string 'SupplierDatabase' is not configured.");

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplierProductRepository, SupplierProductRepository>();

        services.AddMessaging(configuration);

        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSectionOrThrow<RabbitMqSettings>(RabbitMqSettings.SectionName);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CheckSupplierExistsConsumer>();

            x.AddRequestClient<CheckProductExistsRequest>(
                new Uri("queue:check-product-exists"));

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

        services.AddScoped<IProductExistenceChecker, ProductExistenceChecker>();

        return services;
    }
}
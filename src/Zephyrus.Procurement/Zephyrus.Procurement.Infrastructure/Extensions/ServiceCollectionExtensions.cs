using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Infrastructure.Messaging;
using Zephyrus.Procurement.Infrastructure.Persistence;
using Zephyrus.Procurement.Infrastructure.Persistence.Repositories;
using Zephyrus.Procurement.Infrastructure.Settings;
using Zephyrus.SharedKernel.Common.Extensions;
using Zephyrus.SharedKernel.Contracts.Catalog;
using Zephyrus.SharedKernel.Contracts.Supplier;

namespace Zephyrus.Procurement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProcurementDatabase")
            ?? throw new InvalidOperationException("Connection string 'ProcurementDatabase' is not configured.");

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddScoped<IPurchaseRequestRepository, PurchaseRequestRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddMessaging(configuration);

        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSectionOrThrow<RabbitMqSettings>(RabbitMqSettings.SectionName);

        services.AddMassTransit(x =>
        {
            x.AddRequestClient<CheckProductExistsRequest>(
                new Uri("exchange:check-product-exists"));

            x.AddRequestClient<CheckSupplierExistsRequest>(
                new Uri("exchange:check-supplier-exists"));

            x.UsingRabbitMq((_, cfg) =>
            {
                cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.Port, "/", h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });
            });
        });

        services.AddScoped<IProductExistenceChecker, ProductExistenceChecker>();
        services.AddScoped<ISupplierExistenceChecker, SupplierExistenceChecker>();

        return services;
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.Identity.Infrastructure.Persistence;
using Zephyrus.Identity.Infrastructure.Persistence.Repositories;
using Zephyrus.Identity.Infrastructure.Services;
using Zephyrus.Identity.Infrastructure.Settings;
using Zephyrus.SharedKernel.Common.Extensions;

namespace Zephyrus.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var identityDatabaseConnectionString = configuration.GetConnectionString("IdentityDatabase")
            ?? throw new InvalidOperationException("Connection string 'IdentityDatabase' is not configured.");

        var jwtSettings = configuration.GetSectionOrThrow<JwtSettings>(JwtSettings.SectionName);

        services.AddScoped<IDbConnectionFactory>(_ => new DbConnectionFactory(identityDatabaseConnectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtService>(_ => new JwtService(jwtSettings));
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
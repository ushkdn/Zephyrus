using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Zephyrus.Catalog.Application.Extensions;
using Zephyrus.Catalog.Infrastructure.Extensions;
using Zephyrus.Catalog.Presentation.Extensions;
using Zephyrus.Catalog.WebApi.Settings;
using Zephyrus.SharedKernel.Common.Extensions;

namespace Zephyrus.Catalog.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLayers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationLayer();
        services.AddInfrastructureLayer(configuration);
        services.AddPresentationLayer();
        services.AddWebApiLayer(configuration);

        return services;
    }

    private static IServiceCollection AddWebApiLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSectionOrThrow<JwtSettings>(JwtSettings.SectionName);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

        services.AddAuthorization();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Zephyrus Catalog API",
                Version = "v1",
                Description = "Catalog service — управление товарами и категориями"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT токен. Пример: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
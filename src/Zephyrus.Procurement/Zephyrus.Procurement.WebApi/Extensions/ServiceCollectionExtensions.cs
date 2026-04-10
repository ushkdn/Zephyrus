using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Zephyrus.Procurement.Application.Extensions;
using Zephyrus.Procurement.Infrastructure.Extensions;
using Zephyrus.Procurement.Presentation.Extensions;
using Zephyrus.Procurement.WebApi.Settings;
using Zephyrus.SharedKernel.Common.Extensions;

namespace Zephyrus.Procurement.WebApi.Extensions;

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
                Title = "Zephyrus Procurement API",
                Version = "v1",
                Description = "Procurement service — заявки на закупку и заказы"
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

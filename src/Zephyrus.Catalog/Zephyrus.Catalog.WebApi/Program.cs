using Serilog;
using Zephyrus.Catalog.Infrastructure;
using Zephyrus.Catalog.WebApi.Extensions;
using Zephyrus.Catalog.WebApi.Middleware;
using Zephyrus.Logger;
using Zephyrus.SharedKernel.Common.Database;
using Zephyrus.SharedKernel.Common.Extensions;

namespace Zephyrus.Catalog.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting Zephyrus.Catalog...");

            var builder = WebApplication.CreateBuilder(args);

            Log.Information("Loading environment variables...");
            builder.LoadEnvironment("CATALOG");

            SerilogFactory.CreateLogger(builder.Services, builder.Configuration);
            builder.Host.UseSerilog();

            builder.Services.AddOpenApi();
            builder.Services.AddLayers(builder.Configuration);

            var app = builder.Build();

            var connectionString = builder.Configuration.GetConnectionString("CatalogDatabase")
                ?? throw new InvalidOperationException("Connection string 'CatalogDatabase' is not configured.");

            MigrationRunner.Run(connectionString, typeof(ICatalogInfrastructureAssemblyMarker).Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zephyrus Catalog API v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Zephyrus.Catalog terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
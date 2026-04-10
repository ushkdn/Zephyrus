using Serilog;
using Zephyrus.Logger;
using Zephyrus.SharedKernel.Common.Database;
using Zephyrus.Supplier.Infrastructure;
using Zephyrus.Supplier.WebApi.Extensions;
using Zephyrus.Supplier.WebApi.Middleware;

namespace Zephyrus.Supplier.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting Zephyrus.Supplier...");

            var builder = WebApplication.CreateBuilder(args);

            SerilogFactory.CreateLogger(builder.Services, builder.Configuration);
            builder.Host.UseSerilog();

            builder.Services.AddOpenApi();
            builder.Services.AddLayers(builder.Configuration);

            var app = builder.Build();

            var connectionString = builder.Configuration.GetConnectionString("SupplierDatabase")
                ?? throw new InvalidOperationException("Connection string 'SupplierDatabase' is not configured.");

            MigrationRunner.Run(connectionString, typeof(ISupplierInfrastructureAssemblyMarker).Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zephyrus Supplier API v1");
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
            Log.Fatal(ex, "Zephyrus.Supplier terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
using Serilog;
using Zephyrus.Logger;
using Zephyrus.Procurement.Infrastructure;
using Zephyrus.Procurement.WebApi.Extensions;
using Zephyrus.Procurement.WebApi.Middleware;
using Zephyrus.SharedKernel.Common.Database;

namespace Zephyrus.Procurement.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting Zephyrus.Procurement...");

            var builder = WebApplication.CreateBuilder(args);

            SerilogFactory.CreateLogger(builder.Services, builder.Configuration);
            builder.Host.UseSerilog();

            builder.Services.AddOpenApi();
            builder.Services.AddLayers(builder.Configuration);

            var app = builder.Build();

            var connectionString = builder.Configuration.GetConnectionString("ProcurementDatabase")
                ?? throw new InvalidOperationException("Connection string 'ProcurementDatabase' is not configured.");

            MigrationRunner.Run(connectionString, typeof(IProcurementInfrastructureAssemblyMarker).Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zephyrus Procurement API v1");
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
            Log.Fatal(ex, "Zephyrus.Procurement terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

using Serilog;
using Zephyrus.Notification.Infrastructure;
using Zephyrus.Notification.WebApi.Extensions;
using Zephyrus.Notification.WebApi.Middleware;
using Zephyrus.Logger;
using Zephyrus.SharedKernel.Common.Database;

namespace Zephyrus.Notification.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting Zephyrus.Notification...");

            var builder = WebApplication.CreateBuilder(args);

            SerilogFactory.CreateLogger(builder.Services, builder.Configuration);
            builder.Host.UseSerilog();

            builder.Services.AddOpenApi();
            builder.Services.AddLayers(builder.Configuration);

            var app = builder.Build();

            var connectionString = builder.Configuration.GetConnectionString("NotificationDatabase")
                ?? throw new InvalidOperationException("Connection string 'NotificationDatabase' is not configured.");

            MigrationRunner.Run(connectionString, typeof(INotificationInfrastructureAssemblyMarker).Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zephyrus Notification API v1");
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
            Log.Fatal(ex, "Zephyrus.Notification terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

using Serilog;
using Zephyrus.Identity.Infrastructure;
using Zephyrus.Identity.WebApi.Extensions;
using Zephyrus.Identity.WebApi.Middleware;
using Zephyrus.Logger;
using Zephyrus.SharedKernel.Common.Database;
using Zephyrus.SharedKernel.Common.Extensions;

namespace Zephyrus.Identity.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        try
        {
            Log.Information("Starting Zephyrus.Identity...");

            var builder = WebApplication.CreateBuilder(args);

            SerilogFactory.CreateLogger(builder.Services, builder.Configuration);
            builder.Host.UseSerilog();

            Log.Information("Loading environment variables...");
            builder.LoadEnvironment("IDENTITY");

            builder.Services.AddOpenApi();
            builder.Services.AddLayers(builder.Configuration);

            var app = builder.Build();

            var identityDatabaseConnectionString = builder.Configuration.GetConnectionString("IdentityDatabase")
                ?? throw new InvalidOperationException("Connection string 'IdentityDatabase' is not configured.");

            MigrationRunner.Run(identityDatabaseConnectionString, typeof(IIdentityInfrastructureAssemblyMarker).Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zephyrus Identity API v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Zephyrus.Identity terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
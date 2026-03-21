using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Zephyrus.Logger;

public static class SerilogFactory
{
    public static Serilog.ILogger CreateLogger(IServiceCollection services, IConfiguration configuration)
    {
        var serilogLogger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(serilogLogger, true);
        });

        return serilogLogger;
    }
}
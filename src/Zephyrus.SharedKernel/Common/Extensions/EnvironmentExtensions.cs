using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Zephyrus.SharedKernel.Common.Extensions;

public static class EnvironmentExtensions
{
    public static IHostApplicationBuilder LoadEnvironment(this IHostApplicationBuilder builder, string serviceName)
    {
        var envFileName = $".env.{builder.Environment.EnvironmentName.ToLowerInvariant()}";
        var envSharedFileName = $".env.shared.{builder.Environment.EnvironmentName.ToLowerInvariant()}";

        var envFilePath = FindEnvFile(envFileName);
        var envSharedFilePath = FindEnvFile(envSharedFileName);

        if (envFilePath is not null)
            Env.Load(envFilePath);

        if (envSharedFilePath is not null)
            Env.Load(envSharedFilePath);

        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddEnvironmentVariables($"{serviceName.ToUpperInvariant()}_");

        return builder;
    }

    private static string? FindEnvFile(string fileName)
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            var path = Path.Combine(directory.FullName, fileName);
            if (File.Exists(path))
                return path;

            directory = directory.Parent;
        }

        return null;
    }
}

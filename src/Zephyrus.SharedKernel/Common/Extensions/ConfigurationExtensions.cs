using Microsoft.Extensions.Configuration;

namespace Zephyrus.SharedKernel.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T GetSectionOrThrow<T>(this IConfiguration configuration, string sectionName)
    {
        var section = configuration.GetSection(sectionName);

        if (!section.Exists())
            throw new ArgumentException($"Section '{sectionName}' not found in configuration.");

        return section.Get<T>()
            ?? throw new ArgumentException($"Failed to bind settings for section '{sectionName}'.");
    }
}
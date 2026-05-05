namespace Zephyrus.Identity.Infrastructure.Settings;

public class EmailSettings
{
    public const string SectionName = "EmailSettings";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;

    public bool UseSsl { get; set; } = false;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
}

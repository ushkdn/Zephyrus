namespace Zephyrus.Identity.Application.Settings;

public class AuthSettings
{
    public const string SectionName = "AuthSettings";

    public int PasswordResetCodeExpirationMinutes { get; set; } = 5;

}
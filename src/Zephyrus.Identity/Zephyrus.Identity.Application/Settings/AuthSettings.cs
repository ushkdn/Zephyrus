namespace Zephyrus.Identity.Application.Settings;

public class AuthSettings
{
    public const string SectionName = "AuthSettings";

    public int PasswordResetCodeExpirationMinutes { get; set; } = 5;

    public int RefreshTokenExpirationDays { get; set; } = 15;
}
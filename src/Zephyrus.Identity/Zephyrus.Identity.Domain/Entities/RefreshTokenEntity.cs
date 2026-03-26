namespace Zephyrus.Identity.Domain.Entities;

public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime DateExpires { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
}
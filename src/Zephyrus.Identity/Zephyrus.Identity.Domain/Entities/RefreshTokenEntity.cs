using Zephyrus.SharedKernel.Common.Entities;

namespace Zephyrus.Identity.Domain.Entities;

public class RefreshTokenEntity : Entity
{
    public Guid UserId { get; set; }

    public string Token { get; set; }

    public DateTime DateExpires { get; set; }
}
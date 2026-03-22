using Zephyrus.Identity.Domain.Entities;

namespace Zephyrus.Identity.Application.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(UserEntity user);

    string GenerateRefreshToken();
}
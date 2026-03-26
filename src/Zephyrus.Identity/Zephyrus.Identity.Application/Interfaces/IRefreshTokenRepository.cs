using Zephyrus.Identity.Domain.Entities;

namespace Zephyrus.Identity.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshTokenEntity?> GetByTokenAsync(string token, CancellationToken cancellationToken);

    Task AddAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken);

    Task UpdateAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken);
}
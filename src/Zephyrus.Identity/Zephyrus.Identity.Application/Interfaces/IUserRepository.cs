using Zephyrus.Identity.Domain.Entities;

namespace Zephyrus.Identity.Application.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> IsExistsByEmailAsync(string email, CancellationToken cancellationToken);

    Task AddAsync(UserEntity user, CancellationToken cancellationToken);

    Task UpdateAsync(UserEntity user, CancellationToken cancellationToken);
}
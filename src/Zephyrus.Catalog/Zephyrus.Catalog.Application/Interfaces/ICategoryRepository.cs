using Zephyrus.Catalog.Domain.Entities;

namespace Zephyrus.Catalog.Application.Interfaces;

public interface ICategoryRepository
{
    Task<CategoryEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> IsExistsByNameAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(CategoryEntity category, CancellationToken cancellationToken);

    Task UpdateAsync(CategoryEntity category, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
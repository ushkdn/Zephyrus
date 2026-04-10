using Zephyrus.Catalog.Domain.Entities;

namespace Zephyrus.Catalog.Application.Interfaces;

public interface IProductRepository
{
    Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ProductEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<IEnumerable<ProductEntity>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(ProductEntity product, CancellationToken cancellationToken);

    Task UpdateAsync(ProductEntity product, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
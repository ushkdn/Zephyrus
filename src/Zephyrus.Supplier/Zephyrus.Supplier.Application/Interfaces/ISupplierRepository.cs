using Zephyrus.Supplier.Domain.Entities;

namespace Zephyrus.Supplier.Application.Interfaces;

public interface ISupplierRepository
{
    Task<SupplierEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<SupplierEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);

    Task AddAsync(SupplierEntity supplier, CancellationToken cancellationToken);

    Task UpdateAsync(SupplierEntity supplier, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
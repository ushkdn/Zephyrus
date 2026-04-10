using Zephyrus.Supplier.Domain.Entities;

namespace Zephyrus.Supplier.Application.Interfaces;

public interface ISupplierProductRepository
{
    Task<SupplierProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<SupplierProductEntity>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken);

    Task<bool> ExistsBySupplierAndProductAsync(Guid supplierId, Guid productId, CancellationToken cancellationToken);

    Task AddAsync(SupplierProductEntity supplierProduct, CancellationToken cancellationToken);

    Task UpdateAsync(SupplierProductEntity supplierProduct, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
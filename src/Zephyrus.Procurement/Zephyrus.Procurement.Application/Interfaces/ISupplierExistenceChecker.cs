namespace Zephyrus.Procurement.Application.Interfaces;

public interface ISupplierExistenceChecker
{
    Task<bool> ExistsAsync(Guid supplierId, CancellationToken cancellationToken);
}

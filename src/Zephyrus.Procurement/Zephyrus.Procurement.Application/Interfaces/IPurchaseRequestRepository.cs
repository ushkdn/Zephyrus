using Zephyrus.Procurement.Domain.Entities;

namespace Zephyrus.Procurement.Application.Interfaces;

public interface IPurchaseRequestRepository
{
    Task<PurchaseRequestEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<PurchaseRequestEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(PurchaseRequestEntity purchaseRequest, CancellationToken cancellationToken);

    Task UpdateAsync(PurchaseRequestEntity purchaseRequest, CancellationToken cancellationToken);
}

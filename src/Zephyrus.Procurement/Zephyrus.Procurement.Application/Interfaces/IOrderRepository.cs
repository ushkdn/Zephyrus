using Zephyrus.Procurement.Domain.Entities;

namespace Zephyrus.Procurement.Application.Interfaces;

public interface IOrderRepository
{
    Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> ExistsByPurchaseRequestIdAsync(Guid purchaseRequestId, CancellationToken cancellationToken);

    Task AddAsync(OrderEntity order, CancellationToken cancellationToken);

    Task UpdateAsync(OrderEntity order, CancellationToken cancellationToken);
}

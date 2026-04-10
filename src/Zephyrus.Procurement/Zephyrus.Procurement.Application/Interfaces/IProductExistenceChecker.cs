namespace Zephyrus.Procurement.Application.Interfaces;

public interface IProductExistenceChecker
{
    Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken);
}

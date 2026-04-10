namespace Zephyrus.Supplier.Application.Interfaces;

public interface IProductExistenceChecker
{
    Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken);
}
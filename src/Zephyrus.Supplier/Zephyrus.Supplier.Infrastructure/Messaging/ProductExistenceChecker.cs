using MassTransit;
using Zephyrus.SharedKernel.Contracts.Catalog;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Infrastructure.Messaging;

public class ProductExistenceChecker(IRequestClient<CheckProductExistsRequest> requestClient) : IProductExistenceChecker
{
    public async Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken)
    {
        var response = await requestClient.GetResponse<CheckProductExistsResponse>(
            new CheckProductExistsRequest(productId),
            cancellationToken);

        return response.Message.Exists;
    }
}
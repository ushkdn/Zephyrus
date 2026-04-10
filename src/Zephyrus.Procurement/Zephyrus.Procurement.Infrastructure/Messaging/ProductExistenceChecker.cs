using MassTransit;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Contracts.Catalog;

namespace Zephyrus.Procurement.Infrastructure.Messaging;

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

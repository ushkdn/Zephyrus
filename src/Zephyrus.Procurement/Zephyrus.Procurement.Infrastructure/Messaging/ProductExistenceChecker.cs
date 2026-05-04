using MassTransit;
using Microsoft.Extensions.Logging;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Contracts.Catalog;

namespace Zephyrus.Procurement.Infrastructure.Messaging;

public class ProductExistenceChecker(IRequestClient<CheckProductExistsRequest> requestClient, ILogger<ProductExistenceChecker> logger) : IProductExistenceChecker
{
    public async Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken)
    {   logger.LogInformation("Sending product exist request with productId: {productId}", productId);
        var response = await requestClient.GetResponse<CheckProductExistsResponse>(
            new CheckProductExistsRequest(productId),
            cancellationToken);
        logger.LogInformation("Received product exist response: {@response}", response);

        return response.Message.Exists;
    }
}

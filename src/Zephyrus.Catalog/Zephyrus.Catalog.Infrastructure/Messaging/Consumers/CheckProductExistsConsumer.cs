using MassTransit;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Contracts.Catalog;
using Microsoft.Extensions.Logging;

namespace Zephyrus.Catalog.Infrastructure.Messaging.Consumers;

public class CheckProductExistsConsumer(IProductRepository productRepository, ILogger<CheckProductExistsConsumer> logger) : IConsumer<CheckProductExistsRequest>
{
    public async Task Consume(ConsumeContext<CheckProductExistsRequest> context)
    {
        logger.LogInformation("Consuming check-product-exist request with productId: {productId}", context.Message.ProductId);
        var product = await productRepository.GetByIdAsync(context.Message.ProductId, context.CancellationToken);
        logger.LogInformation("Found product by id: {id} and body: {@product}", context.Message.ProductId, product);

        await context.RespondAsync(new CheckProductExistsResponse(product is not null));
    }
}

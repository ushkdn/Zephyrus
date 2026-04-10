using MassTransit;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Contracts.Catalog;

namespace Zephyrus.Catalog.Infrastructure.Messaging.Consumers;

public class CheckProductExistsConsumer(IProductRepository productRepository) : IConsumer<CheckProductExistsRequest>
{
    public async Task Consume(ConsumeContext<CheckProductExistsRequest> context)
    {
        var product = await productRepository.GetByIdAsync(context.Message.ProductId, context.CancellationToken);

        await context.RespondAsync(new CheckProductExistsResponse(product is not null));
    }
}

using MassTransit;
using Zephyrus.SharedKernel.Contracts.Supplier;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Infrastructure.Messaging.Consumers;

public class CheckSupplierExistsConsumer(ISupplierRepository supplierRepository) : IConsumer<CheckSupplierExistsRequest>
{
    public async Task Consume(ConsumeContext<CheckSupplierExistsRequest> context)
    {
        var supplier = await supplierRepository.GetByIdAsync(context.Message.SupplierId, context.CancellationToken);

        await context.RespondAsync(new CheckSupplierExistsResponse(supplier is not null));
    }
}
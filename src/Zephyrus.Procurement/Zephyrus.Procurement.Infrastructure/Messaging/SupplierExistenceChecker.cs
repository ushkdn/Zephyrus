using MassTransit;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Contracts.Supplier;

namespace Zephyrus.Procurement.Infrastructure.Messaging;

public class SupplierExistenceChecker(IRequestClient<CheckSupplierExistsRequest> requestClient) : ISupplierExistenceChecker
{
    public async Task<bool> ExistsAsync(Guid supplierId, CancellationToken cancellationToken)
    {
        var response = await requestClient.GetResponse<CheckSupplierExistsResponse>(
            new CheckSupplierExistsRequest(supplierId),
            cancellationToken);

        return response.Message.Exists;
    }
}

using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.RemoveSupplierProduct;

public class RemoveSupplierProductCommandHandler(
    ISupplierProductRepository supplierProductRepository,
    ILogger<RemoveSupplierProductCommandHandler> logger)
    : IRequestHandler<RemoveSupplierProductCommandRequest, HandlerResponse<RemoveSupplierProductCommandResponse>>
{
    public async Task<HandlerResponse<RemoveSupplierProductCommandResponse>> Handle(RemoveSupplierProductCommandRequest request, CancellationToken cancellationToken)
    {
        var supplierProduct = await supplierProductRepository.GetByIdAsync(request.Id, cancellationToken);

        if (supplierProduct is null)
        {
            logger.LogWarning("Supplier product {SupplierProductId} not found", request.Id);
            return new HandlerResponse<RemoveSupplierProductCommandResponse>(null, $"Supplier product with id: {request.Id} not found.", false);
        }

        if (supplierProduct.SupplierId != request.SupplierId)
        {
            logger.LogWarning("Supplier product {SupplierProductId} does not belong to supplier {SupplierId}", request.Id, request.SupplierId);
            return new HandlerResponse<RemoveSupplierProductCommandResponse>(null, $"Supplier product does not belong to supplier with id: {request.SupplierId}.", false);
        }

        await supplierProductRepository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Supplier product {SupplierProductId} removed from supplier {SupplierId}", request.Id, request.SupplierId);

        return new HandlerResponse<RemoveSupplierProductCommandResponse>(
            new RemoveSupplierProductCommandResponse(request.Id),
            "Supplier product removed successfully.",
            true);
    }
}

using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.UpdateSupplierProduct;

public class UpdateSupplierProductCommandHandler(
    ISupplierProductRepository supplierProductRepository,
    ILogger<UpdateSupplierProductCommandHandler> logger)
    : IRequestHandler<UpdateSupplierProductCommandRequest, HandlerResponse<UpdateSupplierProductCommandResponse>>
{
    public async Task<HandlerResponse<UpdateSupplierProductCommandResponse>> Handle(UpdateSupplierProductCommandRequest request, CancellationToken cancellationToken)
    {
        var supplierProduct = await supplierProductRepository.GetByIdAsync(request.Id, cancellationToken);

        if (supplierProduct is null)
        {
            logger.LogWarning("Supplier product {SupplierProductId} not found", request.Id);
            return new HandlerResponse<UpdateSupplierProductCommandResponse>(null, $"Supplier product with id: {request.Id} not found.", false);
        }

        if (supplierProduct.SupplierId != request.SupplierId)
        {
            logger.LogWarning("Supplier product {SupplierProductId} does not belong to supplier {SupplierId}", request.Id, request.SupplierId);
            return new HandlerResponse<UpdateSupplierProductCommandResponse>(null, $"Supplier product does not belong to supplier with id: {request.SupplierId}.", false);
        }

        supplierProduct.Price = request.Price;
        supplierProduct.Currency = request.Currency.Trim().ToUpper();
        supplierProduct.IsAvailable = request.IsAvailable;
        supplierProduct.DateUpdated = DateTime.UtcNow;

        await supplierProductRepository.UpdateAsync(supplierProduct, cancellationToken);

        logger.LogInformation("Supplier product {SupplierProductId} updated", supplierProduct.Id);

        return new HandlerResponse<UpdateSupplierProductCommandResponse>(
            new UpdateSupplierProductCommandResponse(supplierProduct.Id, supplierProduct.SupplierId, supplierProduct.ProductId, supplierProduct.Price, supplierProduct.Currency, supplierProduct.IsAvailable),
            "Supplier product updated successfully.",
            true);
    }
}

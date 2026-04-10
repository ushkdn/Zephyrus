using MediatR;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.RemoveSupplierProduct;

public class RemoveSupplierProductCommandHandler(ISupplierProductRepository supplierProductRepository)
    : IRequestHandler<RemoveSupplierProductCommandRequest, HandlerResponse<RemoveSupplierProductCommandResponse>>
{
    public async Task<HandlerResponse<RemoveSupplierProductCommandResponse>> Handle(RemoveSupplierProductCommandRequest request, CancellationToken cancellationToken)
    {
        var supplierProduct = await supplierProductRepository.GetByIdAsync(request.Id, cancellationToken);

        if (supplierProduct is null)
            return new HandlerResponse<RemoveSupplierProductCommandResponse>(null, $"Supplier product with id: {request.Id} not found.", false);

        if (supplierProduct.SupplierId != request.SupplierId)
            return new HandlerResponse<RemoveSupplierProductCommandResponse>(null, $"Supplier product does not belong to supplier with id: {request.SupplierId}.", false);

        await supplierProductRepository.DeleteAsync(request.Id, cancellationToken);

        return new HandlerResponse<RemoveSupplierProductCommandResponse>(
            new RemoveSupplierProductCommandResponse(request.Id),
            "Supplier product removed successfully.",
            true);
    }
}
using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierCommandHandler(
    ISupplierRepository supplierRepository,
    ILogger<DeleteSupplierCommandHandler> logger)
    : IRequestHandler<DeleteSupplierCommandRequest, HandlerResponse<DeleteSupplierCommandResponse>>
{
    public async Task<HandlerResponse<DeleteSupplierCommandResponse>> Handle(DeleteSupplierCommandRequest request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);

        if (supplier is null)
        {
            logger.LogWarning("Supplier {SupplierId} not found", request.Id);
            return new HandlerResponse<DeleteSupplierCommandResponse>(null, $"Supplier with id: {request.Id} not found.", false);
        }

        await supplierRepository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Supplier {SupplierId} deleted", request.Id);

        return new HandlerResponse<DeleteSupplierCommandResponse>(
            new DeleteSupplierCommandResponse(request.Id),
            "Supplier deleted successfully.",
            true);
    }
}

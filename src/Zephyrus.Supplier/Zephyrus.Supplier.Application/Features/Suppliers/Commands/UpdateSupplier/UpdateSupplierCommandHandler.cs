using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandHandler(
    ISupplierRepository supplierRepository,
    ILogger<UpdateSupplierCommandHandler> logger)
    : IRequestHandler<UpdateSupplierCommandRequest, HandlerResponse<UpdateSupplierCommandResponse>>
{
    public async Task<HandlerResponse<UpdateSupplierCommandResponse>> Handle(UpdateSupplierCommandRequest request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);

        if (supplier is null)
        {
            logger.LogWarning("Supplier {SupplierId} not found", request.Id);
            return new HandlerResponse<UpdateSupplierCommandResponse>(null, $"Supplier with id: {request.Id} not found.", false);
        }

        supplier.Name = request.Name.Trim();
        supplier.ContactPerson = request.ContactPerson.Trim();
        supplier.Email = request.Email.Trim();
        supplier.Phone = request.Phone.Trim();
        supplier.IsActive = request.IsActive;
        supplier.DateUpdated = DateTime.UtcNow;

        await supplierRepository.UpdateAsync(supplier, cancellationToken);

        logger.LogInformation("Supplier {SupplierId} updated", supplier.Id);

        return new HandlerResponse<UpdateSupplierCommandResponse>(
            new UpdateSupplierCommandResponse(supplier.Id, supplier.Name, supplier.Email, supplier.IsActive),
            "Supplier updated successfully.",
            true);
    }
}

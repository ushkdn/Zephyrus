using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQueryHandler(
    ISupplierRepository supplierRepository,
    ILogger<GetSupplierByIdQueryHandler> logger)
    : IRequestHandler<GetSupplierByIdQueryRequest, HandlerResponse<GetSupplierByIdQueryResponse>>
{
    public async Task<HandlerResponse<GetSupplierByIdQueryResponse>> Handle(GetSupplierByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);

        if (supplier is null)
        {
            logger.LogWarning("Supplier {SupplierId} not found", request.Id);
            return new HandlerResponse<GetSupplierByIdQueryResponse>(null, $"Supplier with id: {request.Id} not found.", false);
        }

        return new HandlerResponse<GetSupplierByIdQueryResponse>(
            new GetSupplierByIdQueryResponse(supplier.Id, supplier.Name, supplier.ContactPerson, supplier.Email, supplier.Phone, supplier.IsActive),
            null,
            true);
    }
}

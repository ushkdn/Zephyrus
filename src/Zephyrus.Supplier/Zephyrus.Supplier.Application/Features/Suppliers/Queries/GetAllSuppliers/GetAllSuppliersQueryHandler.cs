using MediatR;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetAllSuppliers;

public class GetAllSuppliersQueryHandler(ISupplierRepository supplierRepository)
    : IRequestHandler<GetAllSuppliersQueryRequest, HandlerResponse<IEnumerable<GetAllSuppliersQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetAllSuppliersQueryResponse>>> Handle(GetAllSuppliersQueryRequest request, CancellationToken cancellationToken)
    {
        var suppliers = await supplierRepository.GetAllAsync(cancellationToken);

        var response = suppliers.Select(s =>
            new GetAllSuppliersQueryResponse(s.Id, s.Name, s.ContactPerson, s.Email, s.Phone, s.IsActive));

        return new HandlerResponse<IEnumerable<GetAllSuppliersQueryResponse>>(response, null, true);
    }
}
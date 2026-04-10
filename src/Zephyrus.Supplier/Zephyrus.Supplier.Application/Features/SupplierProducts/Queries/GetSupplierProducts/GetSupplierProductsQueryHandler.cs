using MediatR;
using Zephyrus.SharedKernel.Common;
using Zephyrus.Supplier.Application.Interfaces;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Queries.GetSupplierProducts;

public class GetSupplierProductsQueryHandler(
    ISupplierRepository supplierRepository,
    ISupplierProductRepository supplierProductRepository)
    : IRequestHandler<GetSupplierProductsQueryRequest, HandlerResponse<IEnumerable<GetSupplierProductsQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetSupplierProductsQueryResponse>>> Handle(GetSupplierProductsQueryRequest request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken);

        if (supplier is null)
            return new HandlerResponse<IEnumerable<GetSupplierProductsQueryResponse>>(null, $"Supplier with id: {request.SupplierId} not found.", false);

        var products = await supplierProductRepository.GetBySupplierIdAsync(request.SupplierId, cancellationToken);

        var response = products.Select(p =>
            new GetSupplierProductsQueryResponse(p.Id, p.SupplierId, p.ProductId, p.Price, p.Currency, p.IsAvailable));

        return new HandlerResponse<IEnumerable<GetSupplierProductsQueryResponse>>(response, null, true);
    }
}
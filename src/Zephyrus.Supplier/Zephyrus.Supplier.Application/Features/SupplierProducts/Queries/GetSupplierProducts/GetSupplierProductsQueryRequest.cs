using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Queries.GetSupplierProducts;

public record GetSupplierProductsQueryRequest(Guid SupplierId) : IRequest<HandlerResponse<IEnumerable<GetSupplierProductsQueryResponse>>>;
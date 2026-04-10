using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetAllSuppliers;

public record GetAllSuppliersQueryRequest : IRequest<HandlerResponse<IEnumerable<GetAllSuppliersQueryResponse>>>;
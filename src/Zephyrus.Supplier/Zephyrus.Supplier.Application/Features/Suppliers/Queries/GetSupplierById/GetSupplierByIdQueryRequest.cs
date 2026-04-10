using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetSupplierById;

public record GetSupplierByIdQueryRequest(Guid Id) : IRequest<HandlerResponse<GetSupplierByIdQueryResponse>>;
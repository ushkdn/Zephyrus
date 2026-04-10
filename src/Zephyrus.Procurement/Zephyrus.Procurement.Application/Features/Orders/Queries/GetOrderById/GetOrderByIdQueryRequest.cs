using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQueryRequest(Guid Id) : IRequest<HandlerResponse<GetOrderByIdQueryResponse>>;

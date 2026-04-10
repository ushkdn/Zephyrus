using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetAllOrders;

public record GetAllOrdersQueryRequest : IRequest<HandlerResponse<IEnumerable<GetAllOrdersQueryResponse>>>;

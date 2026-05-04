using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQueryRequest(Guid Id) : IRequest<HandlerResponse<GetUserByIdQueryResponse>>;
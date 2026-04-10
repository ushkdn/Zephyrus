using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQueryRequest(Guid Id) : IRequest<HandlerResponse<GetProductByIdQueryResponse>>;
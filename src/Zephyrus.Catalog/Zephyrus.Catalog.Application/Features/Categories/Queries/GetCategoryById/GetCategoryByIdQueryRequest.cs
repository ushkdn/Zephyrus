using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQueryRequest(Guid Id) : IRequest<HandlerResponse<GetCategoryByIdQueryResponse>>;
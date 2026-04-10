using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQueryRequest : IRequest<HandlerResponse<IEnumerable<GetAllCategoriesQueryResponse>>>;
using MediatR;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetAllCategoriesQueryRequest, HandlerResponse<IEnumerable<GetAllCategoriesQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetAllCategoriesQueryResponse>>> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);

        var response = categories.Select(c => new GetAllCategoriesQueryResponse(
            c.Id, c.Name, c.ParentId, c.DateCreated));

        return new HandlerResponse<IEnumerable<GetAllCategoriesQueryResponse>>(response, null, true);
    }
}
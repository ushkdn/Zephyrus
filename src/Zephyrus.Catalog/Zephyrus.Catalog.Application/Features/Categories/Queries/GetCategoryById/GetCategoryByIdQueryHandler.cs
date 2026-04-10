using MediatR;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoryByIdQueryRequest, HandlerResponse<GetCategoryByIdQueryResponse>>
{
    public async Task<HandlerResponse<GetCategoryByIdQueryResponse>> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return new HandlerResponse<GetCategoryByIdQueryResponse>(null, $"Category with id: {request.Id} not found.", false);

        return new HandlerResponse<GetCategoryByIdQueryResponse>(
            new GetCategoryByIdQueryResponse(category.Id, category.Name, category.ParentId, category.DateCreated, category.DateUpdated),
            null,
            true);
    }
}
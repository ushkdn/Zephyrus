using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(
    ICategoryRepository categoryRepository,
    ILogger<GetCategoryByIdQueryHandler> logger)
    : IRequestHandler<GetCategoryByIdQueryRequest, HandlerResponse<GetCategoryByIdQueryResponse>>
{
    public async Task<HandlerResponse<GetCategoryByIdQueryResponse>> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Category {CategoryId} not found", request.Id);
            return new HandlerResponse<GetCategoryByIdQueryResponse>(null, $"Category with id: {request.Id} not found.", false);
        }

        return new HandlerResponse<GetCategoryByIdQueryResponse>(
            new GetCategoryByIdQueryResponse(category.Id, category.Name, category.ParentId, category.DateCreated, category.DateUpdated),
            null,
            true);
    }
}

using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ILogger<UpdateCategoryCommandHandler> logger)
    : IRequestHandler<UpdateCategoryCommandRequest, HandlerResponse<UpdateCategoryCommandResponse>>
{
    public async Task<HandlerResponse<UpdateCategoryCommandResponse>> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Category {CategoryId} not found", request.Id);
            return new HandlerResponse<UpdateCategoryCommandResponse>(null, $"Category with id: {request.Id} not found.", false);
        }

        category.Name = request.Name.Trim();
        category.ParentId = request.ParentId;
        category.DateUpdated = DateTime.UtcNow;

        await categoryRepository.UpdateAsync(category, cancellationToken);

        logger.LogInformation("Category {CategoryId} updated", category.Id);

        return new HandlerResponse<UpdateCategoryCommandResponse>(
            new UpdateCategoryCommandResponse(category.Id, category.Name, category.ParentId),
            "Category updated successfully.",
            true);
    }
}

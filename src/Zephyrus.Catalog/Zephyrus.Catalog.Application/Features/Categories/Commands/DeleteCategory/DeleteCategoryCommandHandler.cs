using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ILogger<DeleteCategoryCommandHandler> logger)
    : IRequestHandler<DeleteCategoryCommandRequest, HandlerResponse<DeleteCategoryCommandResponse>>
{
    public async Task<HandlerResponse<DeleteCategoryCommandResponse>> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Category {CategoryId} not found", request.Id);
            return new HandlerResponse<DeleteCategoryCommandResponse>(null, $"Category with id: {request.Id} not found.", false);
        }

        await categoryRepository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Category {CategoryId} deleted", request.Id);

        return new HandlerResponse<DeleteCategoryCommandResponse>(
            new DeleteCategoryCommandResponse(request.Id),
            "Category deleted successfully.",
            true);
    }
}

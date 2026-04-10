using MediatR;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<DeleteCategoryCommandRequest, HandlerResponse<DeleteCategoryCommandResponse>>
{
    public async Task<HandlerResponse<DeleteCategoryCommandResponse>> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return new HandlerResponse<DeleteCategoryCommandResponse>(null, $"Category with id: {request.Id} not found.", false);

        await categoryRepository.DeleteAsync(request.Id, cancellationToken);

        return new HandlerResponse<DeleteCategoryCommandResponse>(
            new DeleteCategoryCommandResponse(request.Id),
            "Category deleted successfully.",
            true);
    }
}
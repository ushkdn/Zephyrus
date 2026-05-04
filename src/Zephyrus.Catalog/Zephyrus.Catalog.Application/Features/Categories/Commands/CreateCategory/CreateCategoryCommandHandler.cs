using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.Catalog.Domain.Entities;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ILogger<CreateCategoryCommandHandler> logger)
    : IRequestHandler<CreateCategoryCommandRequest, HandlerResponse<CreateCategoryCommandResponse>>
{
    public async Task<HandlerResponse<CreateCategoryCommandResponse>> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        if (await categoryRepository.IsExistsByNameAsync(request.Name, cancellationToken))
        {
            logger.LogWarning("Category '{Name}' already exists", request.Name);
            return new HandlerResponse<CreateCategoryCommandResponse>(null, $"Category '{request.Name}' already exists.", false);
        }

        var category = new CategoryEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            ParentId = request.ParentId,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await categoryRepository.AddAsync(category, cancellationToken);

        logger.LogInformation("Category {CategoryId} '{Name}' created", category.Id, category.Name);

        return new HandlerResponse<CreateCategoryCommandResponse>(
            new CreateCategoryCommandResponse(category.Id, category.Name),
            $"Category created successfully with id: {category.Id}",
            true);
    }
}

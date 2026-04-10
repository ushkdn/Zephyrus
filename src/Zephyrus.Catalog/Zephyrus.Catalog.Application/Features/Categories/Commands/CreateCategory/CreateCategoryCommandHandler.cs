using MediatR;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.Catalog.Domain.Entities;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<CreateCategoryCommandRequest, HandlerResponse<CreateCategoryCommandResponse>>
{
    public async Task<HandlerResponse<CreateCategoryCommandResponse>> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        if (await categoryRepository.IsExistsByNameAsync(request.Name, cancellationToken))
            return new HandlerResponse<CreateCategoryCommandResponse>(null, $"Category '{request.Name}' already exists.", false);

        var category = new CategoryEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            ParentId = request.ParentId,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await categoryRepository.AddAsync(category, cancellationToken);

        return new HandlerResponse<CreateCategoryCommandResponse>(
            new CreateCategoryCommandResponse(category.Id, category.Name),
            $"Category created successfully with id: {category.Id}",
            true);
    }
}
namespace Zephyrus.Catalog.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommandResponse(Guid Id, string Name, Guid? ParentId);
namespace Zephyrus.Catalog.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQueryResponse(Guid Id, string Name, Guid? ParentId, DateTime DateCreated, DateTime DateUpdated);
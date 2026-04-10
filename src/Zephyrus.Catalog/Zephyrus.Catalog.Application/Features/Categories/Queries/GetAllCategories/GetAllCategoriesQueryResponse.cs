namespace Zephyrus.Catalog.Application.Features.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQueryResponse(Guid Id, string Name, Guid? ParentId, DateTime DateCreated);
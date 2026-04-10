namespace Zephyrus.Catalog.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommandResponse(Guid Id, string Name, string Unit, Guid CategoryId, bool IsActive);
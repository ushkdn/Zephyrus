namespace Zephyrus.Catalog.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommandResponse(Guid Id, string Name, string Unit, Guid CategoryId);
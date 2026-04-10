namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.AddSupplierProduct;

public record AddSupplierProductCommandResponse(Guid Id, Guid SupplierId, Guid ProductId, decimal Price, string Currency);
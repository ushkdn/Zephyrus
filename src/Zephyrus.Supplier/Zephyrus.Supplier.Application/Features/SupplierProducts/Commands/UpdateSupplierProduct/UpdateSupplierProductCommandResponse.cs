namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.UpdateSupplierProduct;

public record UpdateSupplierProductCommandResponse(Guid Id, Guid SupplierId, Guid ProductId, decimal Price, string Currency, bool IsAvailable);
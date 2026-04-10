namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Queries.GetSupplierProducts;

public record GetSupplierProductsQueryResponse(Guid Id, Guid SupplierId, Guid ProductId, decimal Price, string Currency, bool IsAvailable);
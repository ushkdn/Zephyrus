namespace Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetAllSuppliers;

public record GetAllSuppliersQueryResponse(Guid Id, string Name, string ContactPerson, string Email, string Phone, bool IsActive);
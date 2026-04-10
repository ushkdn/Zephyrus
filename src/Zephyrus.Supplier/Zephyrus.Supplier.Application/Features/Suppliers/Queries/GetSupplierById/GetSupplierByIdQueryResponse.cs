namespace Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetSupplierById;

public record GetSupplierByIdQueryResponse(Guid Id, string Name, string ContactPerson, string Email, string Phone, bool IsActive);
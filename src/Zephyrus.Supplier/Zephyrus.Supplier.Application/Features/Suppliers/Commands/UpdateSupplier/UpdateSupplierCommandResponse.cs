namespace Zephyrus.Supplier.Application.Features.Suppliers.Commands.UpdateSupplier;

public record UpdateSupplierCommandResponse(Guid Id, string Name, string Email, bool IsActive);
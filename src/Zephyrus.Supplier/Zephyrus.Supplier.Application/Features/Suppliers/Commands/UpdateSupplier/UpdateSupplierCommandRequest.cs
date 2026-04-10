using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Commands.UpdateSupplier;

public record UpdateSupplierCommandRequest(
    Guid Id,
    string Name,
    string ContactPerson,
    string Email,
    string Phone,
    bool IsActive) : IRequest<HandlerResponse<UpdateSupplierCommandResponse>>;
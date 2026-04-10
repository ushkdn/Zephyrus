using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Commands.CreateSupplier;

public record CreateSupplierCommandRequest(
    string Name,
    string ContactPerson,
    string Email,
    string Phone) : IRequest<HandlerResponse<CreateSupplierCommandResponse>>;
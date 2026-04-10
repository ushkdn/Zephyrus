using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.Suppliers.Commands.DeleteSupplier;

public record DeleteSupplierCommandRequest(Guid Id) : IRequest<HandlerResponse<DeleteSupplierCommandResponse>>;
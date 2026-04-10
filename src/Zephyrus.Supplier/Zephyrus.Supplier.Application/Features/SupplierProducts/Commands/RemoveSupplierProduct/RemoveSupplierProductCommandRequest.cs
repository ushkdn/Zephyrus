using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.RemoveSupplierProduct;

public record RemoveSupplierProductCommandRequest(Guid Id, Guid SupplierId) : IRequest<HandlerResponse<RemoveSupplierProductCommandResponse>>;
using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.UpdateSupplierProduct;

public record UpdateSupplierProductCommandRequest(
    Guid Id,
    Guid SupplierId,
    decimal Price,
    string Currency,
    bool IsAvailable) : IRequest<HandlerResponse<UpdateSupplierProductCommandResponse>>;
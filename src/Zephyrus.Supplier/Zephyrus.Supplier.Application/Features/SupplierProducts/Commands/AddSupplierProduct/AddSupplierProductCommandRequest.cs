using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.AddSupplierProduct;

public record AddSupplierProductCommandRequest(
    Guid SupplierId,
    Guid ProductId,
    decimal Price,
    string Currency) : IRequest<HandlerResponse<AddSupplierProductCommandResponse>>;
using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommandRequest(
    Guid Id,
    string Name,
    string Description,
    string Unit,
    Guid CategoryId,
    bool IsActive
) : IRequest<HandlerResponse<UpdateProductCommandResponse>>;
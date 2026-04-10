using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommandRequest(
    string Name,
    string Description,
    string Unit,
    Guid CategoryId
) : IRequest<HandlerResponse<CreateProductCommandResponse>>;
using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommandRequest(Guid Id) : IRequest<HandlerResponse<DeleteProductCommandResponse>>;
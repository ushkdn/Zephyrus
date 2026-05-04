using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(
    IProductRepository productRepository,
    ILogger<DeleteProductCommandHandler> logger)
    : IRequestHandler<DeleteProductCommandRequest, HandlerResponse<DeleteProductCommandResponse>>
{
    public async Task<HandlerResponse<DeleteProductCommandResponse>> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product {ProductId} not found", request.Id);
            return new HandlerResponse<DeleteProductCommandResponse>(null, $"Product with id: {request.Id} not found.", false);
        }

        await productRepository.DeleteAsync(request.Id, cancellationToken);

        logger.LogInformation("Product {ProductId} deleted", request.Id);

        return new HandlerResponse<DeleteProductCommandResponse>(
            new DeleteProductCommandResponse(request.Id),
            "Product deleted successfully.",
            true);
    }
}

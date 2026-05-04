using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    ILogger<UpdateProductCommandHandler> logger)
    : IRequestHandler<UpdateProductCommandRequest, HandlerResponse<UpdateProductCommandResponse>>
{
    public async Task<HandlerResponse<UpdateProductCommandResponse>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product {ProductId} not found", request.Id);
            return new HandlerResponse<UpdateProductCommandResponse>(null, $"Product with id: {request.Id} not found.", false);
        }

        var categoryExists = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (categoryExists is null)
        {
            logger.LogWarning("Category {CategoryId} not found when updating product {ProductId}", request.CategoryId, request.Id);
            return new HandlerResponse<UpdateProductCommandResponse>(null, $"Category with id: {request.CategoryId} not found.", false);
        }

        product.Name = request.Name.Trim();
        product.Description = request.Description.Trim();
        product.Unit = request.Unit.Trim();
        product.CategoryId = request.CategoryId;
        product.IsActive = request.IsActive;
        product.DateUpdated = DateTime.UtcNow;

        await productRepository.UpdateAsync(product, cancellationToken);

        logger.LogInformation("Product {ProductId} updated", product.Id);

        return new HandlerResponse<UpdateProductCommandResponse>(
            new UpdateProductCommandResponse(product.Id, product.Name, product.Unit, product.CategoryId, product.IsActive),
            "Product updated successfully.",
            true);
    }
}

using MediatR;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository)
    : IRequestHandler<UpdateProductCommandRequest, HandlerResponse<UpdateProductCommandResponse>>
{
    public async Task<HandlerResponse<UpdateProductCommandResponse>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return new HandlerResponse<UpdateProductCommandResponse>(null, $"Product with id: {request.Id} not found.", false);

        var categoryExists = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (categoryExists is null)
            return new HandlerResponse<UpdateProductCommandResponse>(null, $"Category with id: {request.CategoryId} not found.", false);

        product.Name = request.Name.Trim();
        product.Description = request.Description.Trim();
        product.Unit = request.Unit.Trim();
        product.CategoryId = request.CategoryId;
        product.IsActive = request.IsActive;
        product.DateUpdated = DateTime.UtcNow;

        await productRepository.UpdateAsync(product, cancellationToken);

        return new HandlerResponse<UpdateProductCommandResponse>(
            new UpdateProductCommandResponse(product.Id, product.Name, product.Unit, product.CategoryId, product.IsActive),
            "Product updated successfully.",
            true);
    }
}
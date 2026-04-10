using MediatR;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.Catalog.Domain.Entities;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository)
    : IRequestHandler<CreateProductCommandRequest, HandlerResponse<CreateProductCommandResponse>>
{
    public async Task<HandlerResponse<CreateProductCommandResponse>> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var categoryExists = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (categoryExists is null)
            return new HandlerResponse<CreateProductCommandResponse>(null, $"Category with id: {request.CategoryId} not found.", false);

        if (await productRepository.ExistsByNameAsync(request.Name, cancellationToken))
            return new HandlerResponse<CreateProductCommandResponse>(null, $"Product '{request.Name}' already exists.", false);

        var product = new ProductEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Unit = request.Unit.Trim(),
            CategoryId = request.CategoryId,
            IsActive = true,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await productRepository.AddAsync(product, cancellationToken);

        return new HandlerResponse<CreateProductCommandResponse>(
            new CreateProductCommandResponse(product.Id, product.Name, product.Unit, product.CategoryId),
            $"Product created successfully with id: {product.Id}",
            true);
    }
}
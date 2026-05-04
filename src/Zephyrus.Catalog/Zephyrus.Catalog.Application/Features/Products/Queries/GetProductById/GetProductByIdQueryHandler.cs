using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(
    IProductRepository productRepository,
    ILogger<GetProductByIdQueryHandler> logger)
    : IRequestHandler<GetProductByIdQueryRequest, HandlerResponse<GetProductByIdQueryResponse>>
{
    public async Task<HandlerResponse<GetProductByIdQueryResponse>> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product {ProductId} not found", request.Id);
            return new HandlerResponse<GetProductByIdQueryResponse>(null, $"Product with id: {request.Id} not found.", false);
        }

        return new HandlerResponse<GetProductByIdQueryResponse>(
            new GetProductByIdQueryResponse(
                product.Id, product.Name, product.Description, product.Unit,
                product.CategoryId, product.IsActive, product.DateCreated, product.DateUpdated),
            null,
            true);
    }
}

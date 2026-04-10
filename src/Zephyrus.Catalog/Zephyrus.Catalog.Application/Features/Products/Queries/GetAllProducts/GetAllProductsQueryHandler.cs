using MediatR;
using Zephyrus.Catalog.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQueryRequest, HandlerResponse<IEnumerable<GetAllProductsQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetAllProductsQueryResponse>>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);

        var response = products.Select(p => new GetAllProductsQueryResponse(
            p.Id, p.Name, p.Unit, p.CategoryId, p.IsActive));

        return new HandlerResponse<IEnumerable<GetAllProductsQueryResponse>>(response, null, true);
    }
}
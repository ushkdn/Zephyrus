using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Catalog.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQueryRequest : IRequest<HandlerResponse<IEnumerable<GetAllProductsQueryResponse>>>;
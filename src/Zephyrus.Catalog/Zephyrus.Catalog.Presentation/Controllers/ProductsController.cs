using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zephyrus.Catalog.Application.Features.Products.Commands.CreateProduct;
using Zephyrus.Catalog.Application.Features.Products.Commands.DeleteProduct;
using Zephyrus.Catalog.Application.Features.Products.Commands.UpdateProduct;
using Zephyrus.Catalog.Application.Features.Products.Queries.GetAllProducts;
using Zephyrus.Catalog.Application.Features.Products.Queries.GetProductById;

namespace Zephyrus.Catalog.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/products")]
public class ProductsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllProductsQueryRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProductByIdQueryRequest(id), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { Id = id }, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteProductCommandRequest(id), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zephyrus.Catalog.Application.Features.Products.Commands.CreateProduct;
using Zephyrus.Catalog.Application.Features.Products.Commands.DeleteProduct;
using Zephyrus.Catalog.Application.Features.Products.Commands.UpdateProduct;
using Zephyrus.Catalog.Application.Features.Products.Queries.GetAllProducts;
using Zephyrus.Catalog.Application.Features.Products.Queries.GetProductById;

namespace Zephyrus.Catalog.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/products")]
public class ProductsController(ISender sender, ILogger<ProductsController> logger) : ControllerBase
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
        {
            logger.LogWarning("Product {ProductId} not found", id);
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to create product '{Name}' — {Message}", request.Name, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Product '{Name}' created", request.Name);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { Id = id }, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to update product {ProductId} — {Message}", id, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Product {ProductId} updated", id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteProductCommandRequest(id), cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to delete product {ProductId} — {Message}", id, result.Message);
            return NotFound(result);
        }

        logger.LogInformation("Product {ProductId} deleted", id);
        return Ok(result);
    }
}

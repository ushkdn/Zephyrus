using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.AddSupplierProduct;
using Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.RemoveSupplierProduct;
using Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.UpdateSupplierProduct;
using Zephyrus.Supplier.Application.Features.SupplierProducts.Queries.GetSupplierProducts;

namespace Zephyrus.Supplier.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/suppliers/{supplierId:guid}/products")]
public class SupplierProductsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBySupplier(Guid supplierId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSupplierProductsQueryRequest(supplierId), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid supplierId, [FromBody] AddSupplierProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { SupplierId = supplierId }, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid supplierId, Guid id,
        [FromBody] UpdateSupplierProductCommandRequest request,
        CancellationToken cancellationToken
        )
    {
        var result = await sender.Send(request with { Id = id, SupplierId = supplierId }, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remove(Guid supplierId, Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RemoveSupplierProductCommandRequest(id, supplierId), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zephyrus.Supplier.Application.Features.Suppliers.Commands.CreateSupplier;
using Zephyrus.Supplier.Application.Features.Suppliers.Commands.DeleteSupplier;
using Zephyrus.Supplier.Application.Features.Suppliers.Commands.UpdateSupplier;
using Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetAllSuppliers;
using Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetSupplierById;

namespace Zephyrus.Supplier.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/suppliers")]
public class SuppliersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllSuppliersQueryRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSupplierByIdQueryRequest(id), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { Id = id }, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteSupplierCommandRequest(id), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
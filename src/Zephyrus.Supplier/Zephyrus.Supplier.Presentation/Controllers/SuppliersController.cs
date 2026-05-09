using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zephyrus.Supplier.Application.Features.Suppliers.Commands.CreateSupplier;
using Zephyrus.Supplier.Application.Features.Suppliers.Commands.DeleteSupplier;
using Zephyrus.Supplier.Application.Features.Suppliers.Commands.UpdateSupplier;
using Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetAllSuppliers;
using Zephyrus.Supplier.Application.Features.Suppliers.Queries.GetSupplierById;

namespace Zephyrus.Supplier.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/suppliers")]
public class SuppliersController(ISender sender, ILogger<SuppliersController> logger) : ControllerBase
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
        {
            logger.LogWarning("Supplier {SupplierId} not found", id);
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSupplierCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to create supplier '{Name}' — {Message}", request.Name, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Supplier '{Name}' created", request.Name);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { Id = id }, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to update supplier {SupplierId} — {Message}", id, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Supplier {SupplierId} updated", id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteSupplierCommandRequest(id), cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to delete supplier {SupplierId} — {Message}", id, result.Message);
            return NotFound(result);
        }

        logger.LogInformation("Supplier {SupplierId} deleted", id);
        return Ok(result);
    }
}

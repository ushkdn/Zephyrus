using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.ApprovePurchaseRequest;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.CreatePurchaseRequest;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.RejectPurchaseRequest;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetAllPurchaseRequests;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetPurchaseRequestById;

namespace Zephyrus.Procurement.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/purchase-requests")]
public class PurchaseRequestsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllPurchaseRequestsQueryRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPurchaseRequestByIdQueryRequest(id), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await sender.Send(request with { RequestedBy = userId }, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ApprovePurchaseRequestCommandRequest(id), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/reject")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectPurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { Id = id }, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

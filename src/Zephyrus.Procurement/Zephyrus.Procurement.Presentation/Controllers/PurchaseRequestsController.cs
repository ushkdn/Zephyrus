using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.ApprovePurchaseRequest;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.CreatePurchaseRequest;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.RejectPurchaseRequest;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetAllPurchaseRequests;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetMyPurchaseRequests;
using Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetPurchaseRequestById;

namespace Zephyrus.Procurement.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/purchase-requests")]
public class PurchaseRequestsController(ISender sender, ILogger<PurchaseRequestsController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllPurchaseRequestsQueryRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetMy(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await sender.Send(new GetMyPurchaseRequestsQueryRequest(userId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPurchaseRequestByIdQueryRequest(id), cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Purchase request {RequestId} not found", id);
            return NotFound(result);
        }

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role == "Buyer" && result.Data!.RequestedBy != userId)
        {
            logger.LogWarning("User {UserId} attempted to access purchase request {RequestId} belonging to another user", userId, id);
            return Forbid();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await sender.Send(request with { RequestedBy = userId }, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("User {UserId} failed to create purchase request — {Message}", userId, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("User {UserId} created purchase request for product {ProductId}", userId, request.ProductId);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ApprovePurchaseRequestCommandRequest(id), cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to approve purchase request {RequestId} — {Message}", id, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Purchase request {RequestId} approved", id);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/reject")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectPurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { Id = id }, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to reject purchase request {RequestId} — {Message}", id, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Purchase request {RequestId} rejected", id);
        return Ok(result);
    }
}

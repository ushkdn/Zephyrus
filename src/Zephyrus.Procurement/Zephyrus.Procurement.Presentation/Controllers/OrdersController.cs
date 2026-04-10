using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zephyrus.Procurement.Application.Features.Orders.Commands.CancelOrder;
using Zephyrus.Procurement.Application.Features.Orders.Commands.ConfirmOrder;
using Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;
using Zephyrus.Procurement.Application.Features.Orders.Commands.DeliverOrder;
using Zephyrus.Procurement.Application.Features.Orders.Queries.GetAllOrders;
using Zephyrus.Procurement.Application.Features.Orders.Queries.GetOrderById;

namespace Zephyrus.Procurement.Presentation.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Manager")]
[Route("api/orders")]
public class OrdersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllOrdersQueryRequest(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetOrderByIdQueryRequest(id), cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await sender.Send(request with { CreatedBy = userId }, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ConfirmOrderCommandRequest(id), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/deliver")]
    public async Task<IActionResult> Deliver(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeliverOrderCommandRequest(id), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelOrderCommandRequest(id), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

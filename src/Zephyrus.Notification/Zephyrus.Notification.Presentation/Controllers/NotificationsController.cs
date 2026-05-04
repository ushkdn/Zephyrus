using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zephyrus.Notification.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Zephyrus.Notification.Application.Features.Notifications.Queries.GetNotificationsByRecipient;

namespace Zephyrus.Notification.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/notifications")]
public class NotificationsController(ISender sender, ILogger<NotificationsController> logger) : ControllerBase
{
    [HttpGet("{recipientId:guid}")]
    public async Task<IActionResult> GetByRecipient(Guid recipientId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetNotificationsByRecipientQueryRequest(recipientId), cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new MarkNotificationAsReadCommandRequest(id), cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Failed to mark notification {NotificationId} as read — {Message}", id, result.Message);
            return BadRequest(result);
        }

        return Ok(result);
    }
}

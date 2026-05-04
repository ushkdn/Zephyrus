using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zephyrus.Identity.Application.Features.Users.Queries.GetUserById;

namespace Zephyrus.Identity.Presentation.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(ISender sender, ILogger<UserController> logger) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get user attempt for id: {id}", id);
        var command = new GetUserByIdQueryRequest(id);

        var result = await sender.Send(command, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Get user failed for id: {id}", id);
            return NotFound(result);
        }

        logger.LogInformation("Get user successfully for id: {id}", id);
        return Ok(result);
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zephyrus.Identity.Application.Features.Auth.Commands.RefreshToken;
using Zephyrus.Identity.Application.Features.Auth.Commands.SignIn;
using Zephyrus.Identity.Application.Features.Auth.Commands.SignUp;

namespace Zephyrus.Identity.Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }
}
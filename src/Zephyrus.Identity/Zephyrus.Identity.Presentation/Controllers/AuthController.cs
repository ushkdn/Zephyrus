using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zephyrus.Identity.Application.Features.Auth.Commands.ForgotPassword;
using Zephyrus.Identity.Application.Features.Auth.Commands.RefreshToken;
using Zephyrus.Identity.Application.Features.Auth.Commands.ResetPassword;
using Zephyrus.Identity.Application.Features.Auth.Commands.SignIn;
using Zephyrus.Identity.Application.Features.Auth.Commands.SignUp;

namespace Zephyrus.Identity.Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommandRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Sign-up attempt for email: {Email}", request.Email);
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Sign-up failed for email: {Email} — {Message}", request.Email, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Sign-up successful for email: {Email}", request.Email);
        return Ok(result);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommandRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Sign-in attempt for email: {Email}", request.Email);
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Sign-in failed for email: {Email} — {Message}", request.Email, result.Message);
            return Unauthorized(result);
        }

        logger.LogInformation("Sign-in successful for email: {Email}", request.Email);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Token refresh failed — {Message}", result.Message);
            return Unauthorized(result);
        }

        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Forgot password request failed with email: {Email} — {Message}", request.Email, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Forgot password successful for email: {Email}", request.Email);
        return Ok(result);
    }

    [HttpPost("{id:guid}/reset-password")]
    public async Task<IActionResult> ResetPassword([FromRoute] Guid id, [FromBody] ResetPasswordCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(request with { Id = id }, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning("Reset password failed for id: {id} — {Message}", id, result.Message);
            return BadRequest(result);
        }

        logger.LogInformation("Reset password successful for id: {id}", id);
        return Ok(result);
    }

}
using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.Identity.Application.Models;
using Zephyrus.Identity.Application.Settings;
using Zephyrus.SharedKernel.Common;
using Zephyrus.SharedKernel.Common.Cache;
using Zephyrus.SharedKernel.Common.Helpers;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(
    IUserRepository userRepository,
    ICacheService cacheService,
    IEmailService emailService,
    AuthSettings authSettings,
    ILogger<ForgotPasswordCommandHandler> logger)
    : IRequestHandler<ForgotPasswordCommandRequest, HandlerResponse<Guid?>>
{
    public async Task<HandlerResponse<Guid?>> Handle(ForgotPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("Forgot password request for non-existent email: {Email}", request.Email);
            return new HandlerResponse<Guid?>(null, "If this email exists, a reset code has been sent.", true);
        }

        var cacheKey = $"password-reset:{user.Id}";
        var existingCode = await cacheService.GetAsync<string>(cacheKey, cancellationToken);

        if (existingCode is not null)
        {
            logger.LogWarning("Forgot password request rate limited for user: {UserId}", user.Id);
            return new HandlerResponse<Guid?>(null, "Reset code already sent. Please wait before requesting a new one.", false);
        }

        var code = CodeGenerator.GenerateCode(6);
        var expiration = TimeSpan.FromMinutes(authSettings.PasswordResetCodeExpirationMinutes);

        await cacheService.SetAsync(cacheKey, code, expiration, cancellationToken);

        var message = new NotificationMessage(
            To: user.Email,
            Subject: "Password Reset Code",
            Body: $"Your password reset code is: <b>{code}</b>. It expires in {authSettings.PasswordResetCodeExpirationMinutes} minutes.",
            From: null
            );

        await emailService.SendAsync(message, cancellationToken);

        logger.LogInformation("Password reset code sent for user: {UserId}", user.Id);

        return new HandlerResponse<Guid?>(user.Id, "If this email exists, a reset code has been sent.", true);
    }
}
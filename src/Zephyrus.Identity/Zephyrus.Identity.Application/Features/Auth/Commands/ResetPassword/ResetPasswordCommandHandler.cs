using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.SharedKernel.Common;
using Zephyrus.SharedKernel.Common.Cache;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    ICacheService cacheService,
    IPasswordHasher passwordHasher,
    ILogger<ResetPasswordCommandHandler> logger)
    : IRequestHandler<ResetPasswordCommandRequest, HandlerResponse<Unit>>
{
    public async Task<HandlerResponse<Unit>> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = $"password-reset:{request.Id}";
        var storedCode = await cacheService.GetAsync<string>(cacheKey, cancellationToken);

        if (storedCode is null)
        {
            logger.LogWarning("Reset password attempt with expired or missing code for user: {UserId}", request.Id);
            return new HandlerResponse<Unit>(Unit.Value,"Reset code is invalid or has expired.", false);
        }

        if (storedCode != request.ConfirmationCode)
        {
            logger.LogWarning("Reset password attempt with wrong code for user: {UserId}", request.Id);
            return new HandlerResponse<Unit>(Unit.Value,"Reset code is incorrect.", false);
        }

        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("Reset password attempt for non-existent user: {UserId}", request.Id);
            return new HandlerResponse<Unit>(Unit.Value,"User not found.", false);
        }

        user.Password = passwordHasher.Hash(request.NewPassword);
        user.DateUpdated = DateTime.UtcNow;

        await userRepository.UpdateAsync(user, cancellationToken);
        await cacheService.RemoveAsync(cacheKey, cancellationToken);

        logger.LogInformation("Password reset successfully for user: {UserId}", user.Id);

        return new HandlerResponse<Unit>(Unit.Value, "Password has been reset successfully.", true);
    }
}
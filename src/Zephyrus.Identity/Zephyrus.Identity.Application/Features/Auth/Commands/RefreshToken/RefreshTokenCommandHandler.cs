using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.Identity.Application.Settings;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtService jwtService,
    AuthSettings authSettings,
    ILogger<RefreshTokenCommandHandler> logger)
    : IRequestHandler<RefreshTokenCommandRequest, HandlerResponse<RefreshTokenCommandResponse>>
{
    public async Task<HandlerResponse<RefreshTokenCommandResponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var storedRefreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (storedRefreshToken is null)
        {
            logger.LogWarning("Token refresh attempt with unknown refresh token");
            return new HandlerResponse<RefreshTokenCommandResponse>(null, "Refresh token not found.", false);
        }

        if (DateTime.UtcNow >= storedRefreshToken.DateExpires)
        {
            logger.LogWarning("Token refresh attempt with expired token for user: {UserId}", storedRefreshToken.UserId);
            return new HandlerResponse<RefreshTokenCommandResponse>(null, "Refresh token has expired.", false);
        }

        var storedUser = await userRepository.GetByIdAsync(storedRefreshToken.UserId, cancellationToken);

        if (storedUser is null)
        {
            logger.LogWarning("Token refresh attempt for non-existent user: {UserId}", storedRefreshToken.UserId);
            return new HandlerResponse<RefreshTokenCommandResponse>(null, "User not found.", false);
        }

        if (!storedUser.IsActive)
        {
            logger.LogWarning("Token refresh attempt for deactivated account: {UserId}", storedUser.Id);
            return new HandlerResponse<RefreshTokenCommandResponse>(null, "Account is deactivated.", false);
        }

        var newRawRefreshToken = jwtService.GenerateRefreshToken();

        storedRefreshToken.Token = newRawRefreshToken;
        storedRefreshToken.DateExpires = DateTime.UtcNow.AddDays(authSettings.RefreshTokenExpirationDays);
        storedRefreshToken.DateUpdated = DateTime.UtcNow;

        await refreshTokenRepository.UpdateAsync(storedRefreshToken, cancellationToken);

        var newAccessToken = jwtService.GenerateAccessToken(storedUser);

        logger.LogInformation("Tokens refreshed for user: {UserId}", storedUser.Id);

        return new HandlerResponse<RefreshTokenCommandResponse>(
            new RefreshTokenCommandResponse(newAccessToken, newRawRefreshToken),
            "Tokens refreshed successfully.",
            true);
    }
}

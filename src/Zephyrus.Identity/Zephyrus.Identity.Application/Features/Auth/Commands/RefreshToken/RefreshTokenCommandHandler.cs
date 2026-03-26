using MediatR;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtService jwtService
    ) : IRequestHandler<RefreshTokenCommandRequest, HandlerResponse<RefreshTokenCommandResponse>>
{
    public async Task<HandlerResponse<RefreshTokenCommandResponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var storedRefreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (storedRefreshToken is null)
            return new HandlerResponse<RefreshTokenCommandResponse>(null, "Refresh token not found.", false);

        if (DateTime.UtcNow >= storedRefreshToken.DateExpires)
            return new HandlerResponse<RefreshTokenCommandResponse>(null, "Refresh token has expired.", false);

        var storedUser = await userRepository.GetByIdAsync(storedRefreshToken.UserId, cancellationToken);

        if (storedUser is null)
            return new HandlerResponse<RefreshTokenCommandResponse>(null, "User not found.", false);

        if (!storedUser.IsActive)
            return new HandlerResponse<RefreshTokenCommandResponse>(null, "Account is deactivated.", false);

        var newRawRefreshToken = jwtService.GenerateRefreshToken();

        storedRefreshToken.Token = newRawRefreshToken;
        storedRefreshToken.DateExpires = DateTime.UtcNow.AddDays(15);
        storedRefreshToken.DateUpdated = DateTime.UtcNow;

        await refreshTokenRepository.UpdateAsync(storedRefreshToken, cancellationToken);

        var newAccessToken = jwtService.GenerateAccessToken(storedUser);

        return new HandlerResponse<RefreshTokenCommandResponse>(
            new RefreshTokenCommandResponse(newAccessToken, newRawRefreshToken),
            "Tokens refreshed successfully.",
            true
            );
    }
}
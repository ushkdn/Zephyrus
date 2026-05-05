using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Identity.Application.Interfaces;
using Zephyrus.Identity.Application.Settings;
using Zephyrus.Identity.Domain.Entities;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.SignIn;

public class SignInCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    AuthSettings authSettings,
    ILogger<SignInCommandHandler> logger)
    : IRequestHandler<SignInCommandRequest, HandlerResponse<SignInCommandResponse>>
{
    public async Task<HandlerResponse<SignInCommandResponse>> Handle(SignInCommandRequest request, CancellationToken cancellationToken)
    {
        var storedUser = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (storedUser is null || !passwordHasher.Verify(request.Password, storedUser.Password))
        {
            logger.LogWarning("Failed sign-in attempt for email: {Email}", request.Email);
            return new HandlerResponse<SignInCommandResponse>(null, "Invalid email or password.", false);
        }

        if (!storedUser.IsActive)
        {
            logger.LogWarning("Sign-in attempt for deactivated account: {UserId}", storedUser.Id);
            return new HandlerResponse<SignInCommandResponse>(null, "Account is deactivated.", false);
        }

        var accessToken = jwtService.GenerateAccessToken(storedUser);
        var rawRefreshToken = jwtService.GenerateRefreshToken();

        var refreshTokenToStore = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            UserId = storedUser.Id,
            Token = rawRefreshToken,
            DateExpires = DateTime.UtcNow.AddDays(authSettings.RefreshTokenExpirationDays),
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await refreshTokenRepository.AddAsync(refreshTokenToStore, cancellationToken);

        logger.LogInformation("User {UserId} signed in successfully", storedUser.Id);

        return new HandlerResponse<SignInCommandResponse>(
            new SignInCommandResponse(accessToken, rawRefreshToken),
            $"Refresh token created with id: {refreshTokenToStore.Id}.",
            true);
    }
}

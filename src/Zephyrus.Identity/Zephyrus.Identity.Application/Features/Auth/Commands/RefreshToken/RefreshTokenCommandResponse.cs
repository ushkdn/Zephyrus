namespace Zephyrus.Identity.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommandResponse(string AccessToken, string RefreshToken);
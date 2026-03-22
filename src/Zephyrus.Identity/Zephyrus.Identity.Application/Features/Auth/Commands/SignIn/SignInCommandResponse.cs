namespace Zephyrus.Identity.Application.Features.Auth.Commands.SignIn;

public record SignInCommandResponse(string AccessToken, string RefreshToken);
using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.SignIn;

public record SignInCommandRequest(string Email, string Password) : IRequest<HandlerResponse<SignInCommandResponse>>;
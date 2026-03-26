using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommandRequest(string RefreshToken) : IRequest<HandlerResponse<RefreshTokenCommandResponse>>;
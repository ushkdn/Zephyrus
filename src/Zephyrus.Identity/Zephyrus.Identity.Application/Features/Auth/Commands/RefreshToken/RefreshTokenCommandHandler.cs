using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, HandlerResponse<RefreshTokenCommandResponse>>
{
    public async Task<HandlerResponse<RefreshTokenCommandResponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
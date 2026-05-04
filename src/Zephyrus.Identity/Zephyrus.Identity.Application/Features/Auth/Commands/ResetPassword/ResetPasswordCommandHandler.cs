using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler(ILogger<ResetPasswordCommandHandler> logger) : IRequestHandler<ResetPasswordCommandRequest, HandlerResponse<Unit>>
{
    public async Task<HandlerResponse<Unit>> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
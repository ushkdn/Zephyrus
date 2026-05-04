using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(ILogger<ForgotPasswordCommandHandler> logger) : IRequestHandler<ForgotPasswordCommandRequest, HandlerResponse<Unit>>
{
    public Task<HandlerResponse<Unit>> Handle(ForgotPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
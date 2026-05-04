using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.ResetPassword;

public record ResetPasswordCommandRequest(
    Guid Id,
    string ConfirmationCode,
    string NewPassword,
    string ConfirmPassword
    ) : IRequest<HandlerResponse<Unit>>;
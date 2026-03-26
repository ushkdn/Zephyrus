using MediatR;
using Zephyrus.Identity.Domain.Enums;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.SignUp;

public record SignUpCommandRequest(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string MiddleName,
    string LastName,
    UserRole Role
) : IRequest<HandlerResponse<SignUpCommandResponse>>;
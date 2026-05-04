using FluentValidation;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommandRequest>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.").WithErrorCode("400")
            .EmailAddress().WithMessage("Incorrect email format.").WithErrorCode("400");
    }
}
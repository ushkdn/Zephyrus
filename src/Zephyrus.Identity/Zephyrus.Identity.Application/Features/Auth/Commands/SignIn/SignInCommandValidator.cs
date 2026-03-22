using FluentValidation;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.SignIn;

public class SignInCommandValidator : AbstractValidator<SignInCommandRequest>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.").WithErrorCode("400")
            .EmailAddress().WithMessage("Incorrect email format.").WithErrorCode("400");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty").WithErrorCode("400")
            .Length(5, 20).WithMessage("Password cannot be less than 5 and more than 20 characters").WithErrorCode("400");
    }
}
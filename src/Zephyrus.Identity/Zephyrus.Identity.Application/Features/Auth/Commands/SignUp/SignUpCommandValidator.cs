using FluentValidation;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommandRequest>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.").WithErrorCode("400")
            .EmailAddress().WithMessage("Incorrect email format.").WithErrorCode("400");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty").WithErrorCode("400")
            .Length(5, 20).WithMessage("Password cannot be less than 5 and more than 20 characters").WithErrorCode("400");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty").WithErrorCode("400")
            .Length(2, 20).WithMessage("First name cannot be less than 2 and more than 20 characters").WithErrorCode("400");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty").WithErrorCode("400")
            .Length(2, 20).WithMessage("Last name cannot be less than 2 and more than 20 characters").WithErrorCode("400");

        RuleFor(x => x.MiddleName)
            .NotEmpty().WithMessage("Middle name cannot be empty").WithErrorCode("400")
            .Length(2, 20).WithMessage("Middle name cannot be less than 2 and more than 20 characters").WithErrorCode("400");
    }
}
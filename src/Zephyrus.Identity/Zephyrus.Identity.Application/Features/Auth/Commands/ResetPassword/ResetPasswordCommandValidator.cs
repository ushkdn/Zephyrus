using FluentValidation;

namespace Zephyrus.Identity.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommandRequest>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.").WithErrorCode("400");

        RuleFor(x => x.ConfirmationCode)
            .NotEmpty().WithMessage("Confirmation code is required.").WithErrorCode("400")
            .Length(6).WithMessage("Confirmation code must be 6 digits long.").WithErrorCode("400");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Password cannot be empty.").WithErrorCode("400")
            .Length(5, 20).WithMessage("Password cannot be less than 5 and more than 20 characters.").WithErrorCode("400");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword)
            .WithMessage("Passwords do not match.")
            .WithErrorCode("400");

    }
}
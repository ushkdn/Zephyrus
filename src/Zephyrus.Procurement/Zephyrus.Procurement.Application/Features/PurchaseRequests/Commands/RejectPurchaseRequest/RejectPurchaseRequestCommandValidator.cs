using FluentValidation;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.RejectPurchaseRequest;

public class RejectPurchaseRequestCommandValidator : AbstractValidator<RejectPurchaseRequestCommandRequest>
{
    public RejectPurchaseRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment (rejection reason) is required.")
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.");
    }
}

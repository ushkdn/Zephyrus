using FluentValidation;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.CreatePurchaseRequest;

public class CreatePurchaseRequestCommandValidator : AbstractValidator<CreatePurchaseRequestCommandRequest>
{
    public CreatePurchaseRequestCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.RequestedBy)
            .NotEmpty().WithMessage("RequestedBy is required.");
    }
}

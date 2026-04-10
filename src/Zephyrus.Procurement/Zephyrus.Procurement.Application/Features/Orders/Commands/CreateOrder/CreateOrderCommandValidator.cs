using FluentValidation;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommandRequest>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.PurchaseRequestId)
            .NotEmpty().WithMessage("PurchaseRequestId is required.");

        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("SupplierId is required.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .MaximumLength(10).WithMessage("Currency must not exceed 10 characters.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CreatedBy is required.");
    }
}

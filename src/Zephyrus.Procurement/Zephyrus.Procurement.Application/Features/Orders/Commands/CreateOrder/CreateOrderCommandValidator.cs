using FluentValidation;
using Zephyrus.Procurement.Domain.Enums;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommandRequest>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("SupplierId is required.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CreatedBy is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.PurchaseRequestId)
                .NotEmpty().WithMessage("PurchaseRequestId is required.");

            item.RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.");

            item.RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Must(c => Enum.TryParse<Currency>(c, ignoreCase: true, out _))
                .WithMessage($"Currency must be one of: {string.Join(", ", Enum.GetNames<Currency>())}.");
        });
    }
}

using FluentValidation;

namespace Zephyrus.Supplier.Application.Features.SupplierProducts.Commands.AddSupplierProduct;

public class AddSupplierProductCommandValidator : AbstractValidator<AddSupplierProductCommandRequest>
{
    public AddSupplierProductCommandValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("SupplierId is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .MaximumLength(10).WithMessage("Currency must not exceed 10 characters.");
    }
}
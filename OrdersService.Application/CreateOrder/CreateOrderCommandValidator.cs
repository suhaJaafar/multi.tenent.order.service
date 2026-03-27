using FluentValidation;

namespace OrdersService.Application.CreateOrder;

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Order ID is required");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(c => c.Items)
            .NotNull()
            .WithMessage("Order items are required")
            .NotEmpty()
            .WithMessage("Order must contain at least one item");

        RuleForEach(c => c.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty()
                .WithMessage("Product ID is required");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero");
        });

        RuleFor(c => c.Notes)
            .MaximumLength(1000)
            .When(c => !string.IsNullOrEmpty(c.Notes))
            .WithMessage("Notes cannot exceed 1000 characters");
    }
}


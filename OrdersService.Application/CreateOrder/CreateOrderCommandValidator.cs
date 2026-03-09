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

        RuleFor(c => c.TotalAmount)
            .GreaterThan(0)
            .WithMessage("Total amount must be greater than zero");

        RuleFor(c => c.Notes)
            .MaximumLength(1000)
            .When(c => !string.IsNullOrEmpty(c.Notes))
            .WithMessage("Notes cannot exceed 1000 characters");
    }
}


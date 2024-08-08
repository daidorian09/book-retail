using Application.Constants;

namespace Application.Features.Orders.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(p => p.ShipmentAddress)
            .NotEmpty()
            .MaximumLength(AppConstants.MaxAddressLength);

        RuleFor(p => p.Items)
           .NotEmpty();

        RuleFor(p => p.Total)
            .GreaterThan(AppConstants.MinTotal)
            .WithMessage(AppConstants.InvalidTotalMessage);

    }
}
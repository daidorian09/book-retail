using Application.Constants;
using Application.Features.Orders.CreateOrder;

namespace Application.Features.Books.CreateBook;

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
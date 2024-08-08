using Application.Constants;

namespace Application.Features.Books.UpdateBookStock;

public class UpdateBookStockCommandValidator : AbstractValidator<UpdateBookStockCommand>
{
    public UpdateBookStockCommandValidator()
    {
        RuleFor(p => p.Quantity)
           .GreaterThan(AppConstants.InvalidQuantity)
           .WithMessage(AppConstants.InvalidQuantityMessage);
    }
}
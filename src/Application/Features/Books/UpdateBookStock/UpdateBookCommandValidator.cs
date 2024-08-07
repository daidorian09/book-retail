using Application.Constants;
using Application.Features.Books.UpdateBookStock;

namespace Application.Features.Books.CreateBook;

public class UpdateBookStockCommandValidator : AbstractValidator<UpdateBookStockCommand>
{
    public UpdateBookStockCommandValidator()
    {
        RuleFor(p => p.Quantity)
           .GreaterThan(AppConstants.InvalidQuantity)
           .WithMessage(AppConstants.InvalidQuantityMessage);
    }   
}
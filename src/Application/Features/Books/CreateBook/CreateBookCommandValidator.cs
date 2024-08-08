using Application.Constants;

namespace Application.Features.Books.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(p => p.Author)
            .NotEmpty()
            .MaximumLength(AppConstants.MaxTextLength);

        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(AppConstants.MaxTextLength);

        RuleFor(p => p.ISBN)
            .NotEmpty()
            .Matches(AppConstants.IsbnRegex)
            .WithMessage(AppConstants.InvalidIsbnMessage);

        RuleFor(p => p.Image)
           .NotEmpty()
           .MaximumLength(AppConstants.MaxTextLength);

        RuleFor(p => p.Price)
            .GreaterThan(AppConstants.MinPrice)
            .WithMessage(AppConstants.InvalidPriceMessage);

        RuleFor(p => p.Quantity)
           .GreaterThan(AppConstants.InvalidQuantity)
           .WithMessage(AppConstants.InvalidQuantityMessage);
    }   
}
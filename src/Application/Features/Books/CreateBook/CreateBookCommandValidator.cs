using Application.Constants;
using Domain.Enums;

namespace Application.Features.Books.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    private static readonly HashSet<BookStatus> _allowedBookStatuses = new()
    {
        BookStatus.Created,
    };

    public CreateBookCommandValidator()
    {
        RuleFor(p => p.Author)
            .NotEmpty()
            .MaximumLength(AppConstants.MaxTextLength);

        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(AppConstants.MaxTextLength);

        RuleFor(p => p.ISBN)
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

        RuleFor(p => p.BookStatus)
            .NotNull()
            .Must(IsBookStatusAllowed)
            .WithMessage(AppConstants.InvalidBookStatusMessage);

    }

    private bool IsBookStatusAllowed(string bookStatus)
    {
        return Enum.TryParse(typeof(BookStatus), bookStatus, true, out var parsedBookStatus) &&
             _allowedBookStatuses.Contains((BookStatus)parsedBookStatus);
    }
}
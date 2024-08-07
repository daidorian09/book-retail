using Application.Constants;
using Domain.Enums;

namespace Application.Features.Books.CreateBook;

public class UpdateBookStockCommandValidator : AbstractValidator<UpdateBookStockCommand>
{
    private static readonly HashSet<BookStatus> _allowedBookStatuses = new()
    {
        BookStatus.Created,
    };

    public UpdateBookStockCommandValidator()
    {
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
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
            .MaximumLength(100);

        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(p => p.ISBN)
            .Matches(@"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$")
            .WithMessage("Invalid ISBN format.");

        RuleFor(p => p.Image)
           .NotEmpty()
           .MaximumLength(100);

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage("Price should be greater than 0");

        RuleFor(p => p.Quantity)
           .GreaterThan(-1)
           .WithMessage("Quantity should be greater than -1");

        RuleFor(p => p.BookStatus)
            .NotNull()
            .Must(IsBookStatusAllowed)
            .WithMessage("BookStatus must be Created");

    }

    private bool IsBookStatusAllowed(string bookStatus)
    {
        return Enum.TryParse(typeof(BookStatus), bookStatus, true, out var parsedBookStatus) &&
             _allowedBookStatuses.Contains((BookStatus)parsedBookStatus);
    }
}
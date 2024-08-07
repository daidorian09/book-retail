using Application.Constants;
using Application.Persistence;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Books.CreateBook;

public class CreateBookCommand : IRequest<Result<CreateBookCommandResponse>>
{
    public string Author { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<CreateBookCommandResponse>>
{
    private readonly IRepository<Book> _bookRepository;

    public CreateBookCommandHandler(IRepository<Book> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<CreateBookCommandResponse>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Author = request.Author,
            Image = request.Image,
            BookStatus = BookStatus.Created,
            ISBN = request.ISBN,
            OutOfStock = default,
            Price = request.Price,
            Title = request.Title,
            Quantity = request.Quantity,
            CreatedDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        await _bookRepository.CreateAsync(AppConstants.BookBucket, book.Id.ToString(), book);

        return Result.Ok(new CreateBookCommandResponse { Id = book.Id });
    }
}
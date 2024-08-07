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
    public string BookStatus { get; set; }
}

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<CreateBookCommandResponse>>
{
    public CreateBookCommandHandler()
    {

    }

    public async Task<Result<CreateBookCommandResponse>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var customer = new Book
        {
            Id = Guid.NewGuid(),
            Author = request.Author,
            Image = request.Image,
            BookStatus = Enum.Parse<BookStatus>(request.BookStatus),
            ISBN = request.ISBN,
            OutOfStock = default,
            Price = request.Price,
            Title = request.Title,
            Quantity = request.Quantity,
            CreatedDate = DateTimeOffset.UtcNow
        };

        return Result.Ok(new CreateBookCommandResponse { Id = customer.Id });
    }
}
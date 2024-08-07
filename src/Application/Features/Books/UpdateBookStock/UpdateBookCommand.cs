using Application.Features.Books.UpdateBookStock;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Books.CreateBook;

public class UpdateBookStockCommand : IRequest<Result<UpdateBookStockCommandResponse>>
{
    public int Quantity { get; set; }
    public string BookStatus { get; set; }
}

public class UpdateBookStockCommandHandler : IRequestHandler<UpdateBookStockCommand, Result<UpdateBookStockCommandResponse>>
{
    public UpdateBookStockCommandHandler()
    {

    }

    public async Task<Result<UpdateBookStockCommandResponse>> Handle(UpdateBookStockCommand request, CancellationToken cancellationToken)
    {
        var customer = new Book
        {
            Id = Guid.NewGuid(),
            BookStatus = Enum.Parse<BookStatus>(request.BookStatus),
            Quantity = request.Quantity,
            CreatedDate = DateTimeOffset.UtcNow
        };

        return Result.Ok(new UpdateBookStockCommandResponse { Id = customer.Id });
    }
}
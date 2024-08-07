using Application.Constants;
using Application.Persistence;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Books.UpdateBookStock;

public class UpdateBookStockCommand : IRequest<Result<bool>>
{
    public string Id { get; set; }
    public int Quantity { get; set; }
}

public class UpdateBookStockCommandHandler : IRequestHandler<UpdateBookStockCommand, Result<bool>>
{
    private readonly IRepository<Book> _bookRepository;

    public UpdateBookStockCommandHandler(IRepository<Book> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<bool>> Handle(UpdateBookStockCommand request, CancellationToken cancellationToken)
    {
        var (book, cas) = await _bookRepository.GetByIdWithCasAsync(AppConstants.BookBucket, request.Id);

        if (book is null)
        {
            throw new Exceptions.NotFoundException(AppConstants.BookRecordNotFound);
        }

        var fieldsToUpdate = extractFieldsToUpdate(request, book);

        await _bookRepository.PartialUpdateAsync(AppConstants.BookBucket, book.Id.ToString(), fieldsToUpdate, cas);

        return Result.Ok(true);
    }

    private Dictionary<string, object> extractFieldsToUpdate(UpdateBookStockCommand request, Book book)
    {
        return new Dictionary<string, object>
        {
            { AppConstants.LastModifiedDate, DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
            { AppConstants.QuantityField, request.Quantity },
            { AppConstants.BookStatusField, request.Quantity == 0 ? BookStatus.OutOfStock :  book.BookStatus}
        };
    }
}
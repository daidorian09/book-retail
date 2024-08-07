using Application.Constants;
using Application.Persistence;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Orders.CreateOrder;

public class CreateOrderCommand : IRequest<Result<CreateOrderCommandResponse>>
{
    public CustomerMetaData Customer { get; set; }
    public List<BookMetaData> Items { get; set; }
    public decimal Total { get; set; }
    public string ShipmentAddress { get; set; }
    public string PaymentMethod { get; set; }
    public string OrderStatus { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<CreateOrderCommandResponse>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Book> _bookRepository;

    public CreateOrderCommandHandler(IRepository<Order> orderRepository, IRepository<Book> bookRepository)
    {
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
    }

    public async Task<Result<CreateOrderCommandResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var updateTasks = GetBookUpdateTasks(request);

        await Task.WhenAll(updateTasks);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            Total = request.Total,
            ShipmentAddress = request.ShipmentAddress,
            PaymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod),
            OrderStatus = Enum.Parse<OrderStatus>(request.OrderStatus),
            Customer = request.Customer,
            Items = request.Items,
            CreatedDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        await _orderRepository.CreateAsync(AppConstants.OrderBucket, order.Id.ToString(), order);

        return Result.Ok(new CreateOrderCommandResponse { Id = order.Id });
    }

    private IEnumerable<Task> GetBookUpdateTasks(CreateOrderCommand request)
    {
        var updateTasks = request.Items.Select(async item =>
        {
            // Optimistic lock approach with CAS value
            var (book, cas) = await _bookRepository.GetByIdWithCasAsync(AppConstants.BookBucket, item.Id);

            if (IsBookNotOutOfStock(item, book))
            {
                throw new Exceptions.NotFoundException($"{AppConstants.BookRecordNotFound} : {book?.Id}");
            }

            var fieldsToUpdate = ExtractFieldsToUpdate(item, book);

            var isBookUpdated = await _bookRepository.PartialUpdateAsync(AppConstants.BookBucket, book.Id.ToString(), fieldsToUpdate, cas);

            if (!isBookUpdated)
            {
                throw new Exceptions.BookRetailCaseStudyException();
            }
        });

        return updateTasks;
    }
    private static bool IsBookNotOutOfStock(BookMetaData item, Book? book)
    {
        return book is null || book.Quantity < item.Quantity || book.OutOfStock;
    }

    private static Dictionary<string, object> ExtractFieldsToUpdate(BookMetaData item, Book book)
    {
        var quantity = book.Quantity - item.Quantity;
        var isOutOfStock = quantity == 0;

        var fieldsToUpdate = new Dictionary<string, object>
             {
                { AppConstants.QuantityField, quantity },
                { AppConstants.LastModifiedDateField, DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                { AppConstants.OutOfStockField, isOutOfStock },
                { AppConstants.BookStatusField, isOutOfStock ? BookStatus.OutOfStock : book.BookStatus },
            };
        return fieldsToUpdate;
    }
}
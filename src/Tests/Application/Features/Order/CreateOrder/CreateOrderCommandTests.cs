using Application.Constants;
using Application.Exceptions;
using Application.Features.Customers.CreateCustomer;
using Application.Features.Orders.CreateOrder;
using Application.Persistence;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Enums;
using Moq;
using Shouldly;

namespace Tests.Application.Features.Order.CreateOrder;

public class CreateOrderCommandTests
{
    private readonly IFixture _fixture;
    private readonly CreateOrderCommandHandler _sut;
    private readonly Mock<IRepository<Domain.Entities.Order>> _orderRepository;
    private readonly Mock<IRepository<Book>>  _bookRepository;

    public CreateOrderCommandTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _orderRepository = _fixture.Freeze<Mock<IRepository<Domain.Entities.Order>>>();
        _bookRepository = _fixture.Freeze<Mock<IRepository<Book>>>();
        _sut = _fixture.Create<CreateOrderCommandHandler>();
    }

    [Fact]
    public async Task CreateOrder_ReturnsSucceess()
    {
        var customerId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var book = new Book()
        {
            Author = "test",
            BookStatus = BookStatus.Created,
            CreatedDate = 1,
            Id = bookId,
            OutOfStock = false,
            Quantity = 100,
            Price = 100
        };

        var command = new CreateOrderCommand
        {
            Customer = new (customerId.ToString(), "test", "test"),
            Items = new List<BookMetaData>() { new(book.Id.ToString(), book.Author, book.Title, book.Price, 1, book.BookStatus) },
            OrderStatus = OrderStatus.Created.ToString(),
            PaymentMethod = PaymentMethod.CreditCard.ToString(),
            ShipmentAddress = "test",
            Total = 200
        };

        ulong cas = 9497804238020438090;

        _orderRepository.Setup(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>())).ReturnsAsync(true).Verifiable();
        _bookRepository.Setup(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString())).ReturnsAsync((book, cas)).Verifiable();
        _bookRepository.Setup(c => c.PartialUpdateAsync(AppConstants.BookBucket, bookId.ToString(), It.IsAny<Dictionary<string, object>>(), cas)).ReturnsAsync(true).Verifiable();

        var result = await _sut.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        _orderRepository.Verify(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>()), Times.Once);
        _bookRepository.Verify(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString()), Times.Once);
        _bookRepository.Verify(c => c.PartialUpdateAsync(AppConstants.BookBucket, bookId.ToString(), It.IsAny<Dictionary<string, object>>(), cas), Times.Once);
    }

    [Fact]
    public async Task CreateOrder_WithNotExistingBook_ThrowsException()
    {
        var customerId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var book = new Book()
        {
            Author = "test",
            BookStatus = BookStatus.Created,
            CreatedDate = 1,
            Id = bookId,
            OutOfStock = false,
            Quantity = 100,
            Price = 100
        };

        var command = new CreateOrderCommand
        {
            Customer = new(customerId.ToString(), "test", "test"),
            Items = new List<BookMetaData>() { new(book.Id.ToString(), book.Author, book.Title, book.Price, 1, book.BookStatus) },
            OrderStatus = OrderStatus.Created.ToString(),
            PaymentMethod = PaymentMethod.CreditCard.ToString(),
            ShipmentAddress = "test",
            Total = 200
        };

        ulong cas = 9497804238020438090;

        _orderRepository.Setup(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>())).ReturnsAsync(true).Verifiable();
        _bookRepository.Setup(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString())).ReturnsAsync((default, default)).Verifiable();

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await _sut.Handle(command, CancellationToken.None);
        });

        exception.Message.ShouldBe($"{AppConstants.BookRecordNotFound} : {string.Empty}");
        _orderRepository.Verify(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>()), Times.Once);
        _bookRepository.Verify(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString()), Times.Once);
        _bookRepository.Verify(c => c.PartialUpdateAsync(AppConstants.BookBucket, bookId.ToString(), It.IsAny<Dictionary<string, object>>(), cas), Times.Never);
    }

    [Fact]
    public async Task CreateOrder_WithInSufficientQuantity_ThrowsException()
    {
        var customerId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var book = new Book()
        {
            Author = "test",
            BookStatus = BookStatus.Created,
            CreatedDate = 1,
            Id = bookId,
            OutOfStock = false,
            Quantity = 1,
            Price = 100
        };

        var command = new CreateOrderCommand
        {
            Customer = new(customerId.ToString(), "test", "test"),
            Items = new List<BookMetaData>() { new(book.Id.ToString(), book.Author, book.Title, book.Price, 5, book.BookStatus) },
            OrderStatus = OrderStatus.Created.ToString(),
            PaymentMethod = PaymentMethod.CreditCard.ToString(),
            ShipmentAddress = "test",
            Total = 200
        };

        ulong cas = 9497804238020438090;

        _orderRepository.Setup(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>())).ReturnsAsync(true).Verifiable();
        _bookRepository.Setup(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString())).ReturnsAsync((book, default)).Verifiable();

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await _sut.Handle(command, CancellationToken.None);
        });

        exception.Message.ShouldBe($"{AppConstants.BookRecordNotFound} : {book.Id}");
        _orderRepository.Verify(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>()), Times.Once);
        _bookRepository.Verify(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString()), Times.Once);
        _bookRepository.Verify(c => c.PartialUpdateAsync(AppConstants.BookBucket, bookId.ToString(), It.IsAny<Dictionary<string, object>>(), cas), Times.Never);
    }

    [Fact]
    public async Task CreateOrder_WithOutOfStockTrue_ThrowsException()
    {
        var customerId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var book = new Book()
        {
            Author = "test",
            BookStatus = BookStatus.Created,
            CreatedDate = 1,
            Id = bookId,
            OutOfStock = true,
            Quantity = 100,
            Price = 100
        };

        var command = new CreateOrderCommand
        {
            Customer = new(customerId.ToString(), "test", "test"),
            Items = new List<BookMetaData>() { new(book.Id.ToString(), book.Author, book.Title, book.Price, 1, book.BookStatus) },
            OrderStatus = OrderStatus.Created.ToString(),
            PaymentMethod = PaymentMethod.CreditCard.ToString(),
            ShipmentAddress = "test",
            Total = 200
        };

        ulong cas = 9497804238020438090;

        _orderRepository.Setup(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>())).ReturnsAsync(true).Verifiable();
        _bookRepository.Setup(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString())).ReturnsAsync((book, default)).Verifiable();

        var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await _sut.Handle(command, CancellationToken.None);
        });

        exception.Message.ShouldBe($"{AppConstants.BookRecordNotFound} : {book.Id}");
        _orderRepository.Verify(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>()), Times.Once);
        _bookRepository.Verify(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString()), Times.Once);
        _bookRepository.Verify(c => c.PartialUpdateAsync(AppConstants.BookBucket, bookId.ToString(), It.IsAny<Dictionary<string, object>>(), cas), Times.Never);
    }

    [Fact]
    public async Task CreateOrder_WithPartialUpdateFailure_ThrowsException()
    {
        var customerId = Guid.NewGuid();
        var bookId = Guid.NewGuid();

        var book = new Book()
        {
            Author = "test",
            BookStatus = BookStatus.Created,
            CreatedDate = 1,
            Id = bookId,
            OutOfStock = false,
            Quantity = 100,
            Price = 100
        };

        var command = new CreateOrderCommand
        {
            Customer = new(customerId.ToString(), "test", "test"),
            Items = new List<BookMetaData>() { new(book.Id.ToString(), book.Author, book.Title, book.Price, 1, book.BookStatus) },
            OrderStatus = OrderStatus.Created.ToString(),
            PaymentMethod = PaymentMethod.CreditCard.ToString(),
            ShipmentAddress = "test",
            Total = 200
        };

        ulong cas = 9497804238020438090;

        _orderRepository.Setup(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>())).ReturnsAsync(true).Verifiable();
        _bookRepository.Setup(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString())).ReturnsAsync((book, cas)).Verifiable();
        _bookRepository.Setup(c => c.PartialUpdateAsync(AppConstants.BookBucket, bookId.ToString(), It.IsAny<Dictionary<string, object>>(), cas)).ReturnsAsync(false).Verifiable();

        var exception = await Assert.ThrowsAsync<BookRetailCaseStudyException>(async () =>
        {
            await _sut.Handle(command, CancellationToken.None);
        });

        exception.Message.ShouldNotBeEmpty();
        _orderRepository.Verify(c => c.CreateAsync(AppConstants.OrderBucket, It.IsAny<string>(), It.IsAny<Domain.Entities.Order>()), Times.Once);
        _bookRepository.Verify(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, bookId.ToString()), Times.Once);
        _bookRepository.Verify(c => c.PartialUpdateAsync(AppConstants.BookBucket, bookId.ToString(), It.IsAny<Dictionary<string, object>>(), cas), Times.Once);
    }
}

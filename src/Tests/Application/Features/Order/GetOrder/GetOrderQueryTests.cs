using Application.Constants;
using Application.Exceptions;
using Application.Features.Orders.CreateOrder;
using Application.Persistence;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Shouldly;

namespace Tests.Application.Features.Order.GetOrder
{
    public class GetOrderQueryTests
    {
        private readonly IFixture _fixture;
        private readonly GetOrderQueryHandler _sut;
        private readonly Mock<IRepository<Domain.Entities.Order>> _orderRepository;

        public GetOrderQueryTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _orderRepository = _fixture.Freeze<Mock<IRepository<Domain.Entities.Order>>>();
            _sut = _fixture.Create<GetOrderQueryHandler>();
        }

        [Fact]
        public async Task GetOrders_ReturnsSuccess()
        {
            var id = Guid.NewGuid();

            var command = new GetOrderQuery
            {
                Id = id.ToString(),
            };

            var order = new Domain.Entities.Order()
            {
                Id = id,
                OrderStatus = Domain.Enums.OrderStatus.Created,
                PaymentMethod = Domain.Enums.PaymentMethod.Cash
            };

            _orderRepository.Setup(c => c.GetByIdAsync(AppConstants.OrderBucket, command.Id))
                .ReturnsAsync(order)
                .Verifiable();

            var result = await _sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            result.Value.Id.ShouldBe(command.Id);
            result.Value.OrderStatus.ShouldBe(order.OrderStatus);
            result.Value.PaymentMethod.ShouldBe(order.PaymentMethod);
        }

        [Fact]
        public async Task GetOrders_WithNotExistingOrder_ThrowsException()
        {
            var id = Guid.NewGuid();

            var command = new GetOrderQuery
            {
                Id = id.ToString(),
            };

            _orderRepository.Setup(c => c.GetByIdAsync(AppConstants.BookBucket, command.Id)).ReturnsAsync(It.IsAny<Domain.Entities.Order>).Verifiable();


            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _sut.Handle(command, CancellationToken.None);
            });

            exception.Message.ShouldBe($"{AppConstants.OrderRecordNotFound} : {command.Id}");
        }


    }
}

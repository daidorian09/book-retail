using Application.Constants;
using Application.Exceptions;
using Application.Features.Orders.CreateOrder;
using Application.Features.Orders.GetOrdersWithDate;
using Application.Persistence;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Newtonsoft.Json.Linq;
using Shouldly;

namespace Tests.Application.Features.Order.GetOrder
{
    public class GetOrdersWithDateQueryTests
    {
        private readonly IFixture _fixture;
        private readonly GetOrdersWithQueryHandler _sut;
        private readonly Mock<IRepository<Domain.Entities.Order>> _orderRepository;

        public GetOrdersWithDateQueryTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _orderRepository = _fixture.Freeze<Mock<IRepository<Domain.Entities.Order>>>();
            _sut = _fixture.Create<GetOrdersWithQueryHandler>();
        }

        [Fact]
        public async Task GetOrdersWithDate_ReturnsListOfOrders()
        {
            var query = new GetOrdersWithDateQuery
            {
                StartDate = 1723072377,
                EndDate = 1723072479
            };

            var orderJson = @"
            {
             'order': {
               'customer': {
                 'id': '3c101fc5-24d9-4398-8de3-9976c5587e19',
                 'firstName': 'Test',
              'lastName': 'test'
            },
            'items': [
              {
                'id': '4ef0d7b9-dc15-4892-b3e9-fe9cd6af1060',
                'author': 'Test',
                'title': 'test',
                'price': 10.99,
                'quantity': 1,
                'bookStatus': 0
              },
              {
                'id': '4ab0d7b9-dc15-4893-b3e9-fe9cd6af1020',
                'author': 'Test',
                'title': 'test',
                'price': 10.99,
                'quantity': 1,
                'bookStatus': 0
              }
            ],
            'orderDate': 1723072377,
            'total': 125.78,
            'shipmentAddress': 'test',
            'paymentMethod': 0,
            'orderStatus': 0,
            'id': 'f0e1840d-1b3a-4b0e-8ca2-41146f3f6b47',
            'createdDate': 1723072377,
            'lastModifiedDate': null,
            }
           }";

            var orderAsJObject = JObject.Parse(orderJson);

            var dynamicOrders = new List<dynamic>
            {
                orderAsJObject
            };

            _orderRepository.Setup(c => c.GetWithDateAsync(AppConstants.OrderBucket, AppConstants.OrderDateField, query.StartDate, query.EndDate))
                .ReturnsAsync(dynamicOrders)
                .Verifiable();

            var result = await _sut.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            result.Value.Any().ShouldBeTrue();
        }

        [Fact]
        public async Task GetOrdersWithDate_ReturnsEmptyListOfOrders()
        {
            var id = Guid.NewGuid();

            var query = new GetOrdersWithDateQuery
            {
                StartDate = 1723072377,
                EndDate = 1723072479
            };

            _orderRepository.Setup(c => c.GetWithDateAsync(AppConstants.OrderBucket, AppConstants.OrderDateField, query.StartDate, query.EndDate))
                            .ReturnsAsync(It.IsAny<List<dynamic>>)
                            .Verifiable();

            var result = await _sut.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            result.Value.Any().ShouldBeFalse();
        }


    }
}

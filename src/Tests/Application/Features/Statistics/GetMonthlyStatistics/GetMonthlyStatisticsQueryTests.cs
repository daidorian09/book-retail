using Application.Constants;
using Application.Features.Statistics.GetMonthlyStatistics;
using Application.Persistence;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Newtonsoft.Json;
using Shouldly;

namespace Tests.Application.Features.Statistics.GetMonthlyStatistics
{
    public class GetMonthlyStatisticsQueryTests
    {
        private readonly IFixture _fixture;
        private readonly GetMonthlyStatisticsQueryHandler _sut;
        private readonly Mock<IRepository<Domain.Entities.Order>> _orderRepository;

        public GetMonthlyStatisticsQueryTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _orderRepository = _fixture.Freeze<Mock<IRepository<Domain.Entities.Order>>>();
            _sut = _fixture.Create<GetMonthlyStatisticsQueryHandler>();
        }

        [Fact]
        public async Task GetMonthlyStatisticsQuery_ReturnsEmptyList()
        {
            var query = new GetMonthlyStatisticsQuery
            {
                Year = 1
            };

            _orderRepository.Setup(c => c.GetMonthlyStatisticsAsync(AppConstants.OrderBucket, query.Year))
               .ReturnsAsync(It.IsAny<List<dynamic>>())
               .Verifiable();

            var result = await _sut.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            result.Value.Any().ShouldBeFalse();
        }

        [Fact]
        public async Task GetMonthlyStatisticsQuery_ReturnsList()
        {
            var query = new GetMonthlyStatisticsQuery
            {
                Year = 2024
            };

            var augustMonthlyStatisticsAsJson = @"{ ""month"": ""August"", ""monthlySum"": 249.56, ""totalPurchasedBooks"": 3, ""totalOrderCount"": 2 }";
            var mayMonthlyStatisticsAsJson = @"{ ""month"": ""May"", ""monthlySum"": 105.78, ""totalPurchasedBooks"": 1, ""totalOrderCount"": 1 }";

            var augustMonthlyStatisticsAsDynamic = JsonConvert.DeserializeObject<dynamic>(augustMonthlyStatisticsAsJson);
            var mayMonthlyStatisticsAsDynamic = JsonConvert.DeserializeObject<dynamic>(mayMonthlyStatisticsAsJson);

            var monthlyStatistics = new List<dynamic> { augustMonthlyStatisticsAsDynamic, mayMonthlyStatisticsAsDynamic };

            _orderRepository
                .Setup(repo => repo.GetMonthlyStatisticsAsync(
                    AppConstants.OrderBucket,
                    query.Year))
                .ReturnsAsync(monthlyStatistics);

            var result = await _sut.Handle(query, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            result.Value.Any().ShouldBeTrue();
        }
    }
}

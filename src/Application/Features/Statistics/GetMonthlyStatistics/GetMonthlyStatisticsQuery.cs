using Application.Constants;
using Application.Extensions;
using Application.Persistence;
using Domain.Entities;

namespace Application.Features.Statistics.GetMonthlyStatistics
{
    public class GetMonthlyStatisticsQuery : IRequest<Result<List<GetMonthlyStatisticsQueryResponse>>>
    {
        public int Year { get; set; }
    }

    public class GetMonthlyStatisticsQueryHandler : IRequestHandler<GetMonthlyStatisticsQuery, Result<List<GetMonthlyStatisticsQueryResponse>>>
    {
        private readonly IRepository<Order> _orderRepository;

        public GetMonthlyStatisticsQueryHandler(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<List<GetMonthlyStatisticsQueryResponse>>> Handle(GetMonthlyStatisticsQuery request, CancellationToken cancellationToken)
        {
            var filteredMonthlyStatistics = await _orderRepository.GetMonthlyStatisticsAsync(AppConstants.OrderBucket, request.Year);

            if (!filteredMonthlyStatistics.NotNullOrEmpty())
            {
                return new List<GetMonthlyStatisticsQueryResponse>();
            }

            var monthlyStatistics = filteredMonthlyStatistics
                .Select(row => new GetMonthlyStatisticsQueryResponse
                {
                    Month = row.month,
                    MonthlySum = row.monthlySum,
                    TotalPurchasedBooks = row.totalPurchasedBooks,
                    TotalOrderCount = row.totalOrderCount
                })
                .ToList();

            return Result.Ok(monthlyStatistics);
        }
    }
}

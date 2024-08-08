using Application.Constants;
using Application.Extensions;
using Application.Persistence;
using Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Application.Features.Orders.GetOrdersWithDate
{
    public class GetOrdersWithDateQuery : IRequest<Result<List<GetOrdersWithDateQueryResponse>>>
    {
        public long StartDate { get; set; }
        public long EndDate { get; set; }

    }

    public class GetOrdersWithQueryHandler : IRequestHandler<GetOrdersWithDateQuery, Result<List<GetOrdersWithDateQueryResponse>>>
    {
        private readonly IRepository<Order> _orderRepository;

        public GetOrdersWithQueryHandler(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<List<GetOrdersWithDateQueryResponse>>> Handle(GetOrdersWithDateQuery request, CancellationToken cancellationToken)
        {
            var filteredOrders = await _orderRepository.GetWithDateAsync(AppConstants.OrderBucket, AppConstants.OrderDateField, request.StartDate, request.EndDate);

            if (!filteredOrders.NotNullOrEmpty())
            {
                return new List<GetOrdersWithDateQueryResponse>();
            }

            var orders = filteredOrders
                .Select(row => row[AppConstants.OrderBucket] as JObject)
                .Where(document => document is not null)
                .Select(document => document?.ToObject<GetOrdersWithDateQueryResponse>())
                .Where(order => order is not null)
                .ToList();

            return Result.Ok(orders);
        }
    }
}

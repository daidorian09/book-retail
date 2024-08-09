using Application.Constants;
using Application.Extensions;
using Application.Models;
using Application.Persistence;
using CleanArchitecture.Application.Features.Products.GetPagedProducts;
using Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Application.Features.Customers.GetCustomerOrders
{
    public class GetCustomerOrdersQuery : IRequest<Result<PaginatedList<GetCustomerOrdersQueryResponse>>>
    {
        public string CustomerId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }

    public class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, Result<PaginatedList<GetCustomerOrdersQueryResponse>>>
    {
        private readonly IRepository<Order> _orderRepository;

        public GetCustomerOrdersQueryHandler(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<PaginatedList<GetCustomerOrdersQueryResponse>>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var paginatedOrders = await _orderRepository.GetWithPaginationAsync(AppConstants.OrderBucket, AppConstants.CustomerIdField, request.CustomerId, request.PageNumber, request.PageSize);

            if(!paginatedOrders.NotNullOrEmpty())
            {
                return new PaginatedList<GetCustomerOrdersQueryResponse>(new(), default, default, default);
            }

            var orders = paginatedOrders
                .Select(row => row[AppConstants.OrderBucket] as JObject)
                .Where(document => document is not null)
                .Select(document => document?.ToObject<GetCustomerOrdersQueryResponse>())
                .Where(order => order is not null)
                .ToList();

            var response = new PaginatedList<GetCustomerOrdersQueryResponse>(
            orders,
            orders.Count,
             request.PageNumber,
            request.PageSize);

            return Result.Ok(response);
        }
    }
}

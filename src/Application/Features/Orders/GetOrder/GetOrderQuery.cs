using Application.Constants;
using Application.Persistence;
using Domain.Entities;

namespace Application.Features.Orders.CreateOrder;

public class GetOrderQuery : IRequest<Result<GetOrderQueryResponse>>
{
    public string Id { get; set; }
}

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<GetOrderQueryResponse>>
{
    private readonly IRepository<Order> _orderRepository;

    public GetOrderQueryHandler(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<GetOrderQueryResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
       var order = await _orderRepository.GetByIdAsync(AppConstants.OrderBucket, request.Id) ?? throw new Exceptions.NotFoundException($"{AppConstants.OrderRecordNotFound} : {request.Id}");

        return Result.Ok(new GetOrderQueryResponse { 
            Id = request.Id,
            Customer = order.Customer,
            Items = order.Items,
            OrderDate = order.OrderDate,
            OrderStatus = order.OrderStatus,
            PaymentMethod = order.PaymentMethod,
            ShipmentAddress = order.ShipmentAddress,
            Total = order.Total
        });
    }
}
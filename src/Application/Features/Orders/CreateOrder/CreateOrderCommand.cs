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

    public CreateOrderCommandHandler(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<CreateOrderCommandResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
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
}
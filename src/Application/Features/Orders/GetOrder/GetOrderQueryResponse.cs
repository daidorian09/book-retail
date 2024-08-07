using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Orders.CreateOrder;

public class GetOrderQueryResponse
{
    public string Id { get; set; }
    public CustomerMetaData Customer { get; set; }
    public List<BookMetaData> Items { get; set; }
    public long OrderDate { get; set; }
    public decimal Total { get; set; }
    public string ShipmentAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public OrderStatus OrderStatus { get; set; }
}

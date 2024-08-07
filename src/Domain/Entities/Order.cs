namespace Domain.Entities;

public class Order : BaseEntity
{
    public  CustomerMetaData Customer { get; set; }
    public  BookMetaData BookMetaData { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public  decimal Total { get; set; }
    public  string ShipmentAddress { get; set; }
    public  string PaymentMethod { get; set; }
    public  OrderStatus OrderStatus { get; set; }
}

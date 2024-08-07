namespace Domain.Entities;

public class Order : BaseEntity
{
    public  CustomerMetaData Customer { get; set; }
    public  List<BookMetaData> Items { get; set; }
    public long OrderDate { get; set; }
    public  decimal Total { get; set; }
    public  string ShipmentAddress { get; set; }
    public  PaymentMethod PaymentMethod { get; set; }
    public  OrderStatus OrderStatus { get; set; }
}
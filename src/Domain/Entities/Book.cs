namespace Domain.Entities;

public class Book : BaseEntity
{
    public string Author { get; set; } 
    public string Title { get; set; }
    public string ISBN { get; set; } 
    public string Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool OutOfStock { get; set; }
    public BookStatus BookStatus { get; set; }
}

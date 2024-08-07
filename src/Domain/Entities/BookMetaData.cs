namespace Domain.Entities;

public class BookMetaData
{
    public  string Author { get; set; }
    public  string Title { get; set; } 
    public  decimal Price { get; set; }
    public  int Quantity { get; set; }
    public  BookStatus BookStatus { get; set; }
}

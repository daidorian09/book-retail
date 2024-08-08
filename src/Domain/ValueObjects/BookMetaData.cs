namespace Domain.Entities;

public record BookMetaData(string Id, string Author, string Title, decimal Price, int Quantity, BookStatus BookStatus);

using Domain.Enums;

namespace Application.Constants
{
    public class AppConstants
    {
        public const int MaxTextLength = 100;
        public const int MaxAddressLength = 256;
        public const int MinTotal = 0;
        public const int MinPageNumber = 0;
        public const int MaxPageNumber = 100;
        public const int MinPageSize = 0;
        public const int MaxPageSize = 100;
        public const int InvalidQuantity = -1;
        public const decimal MinPrice = 0;
        public const int MinStartDate = 0;
        public const int MinEndDate = 0;

        public const string IsbnRegex = @"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$";
        public const string InvalidIsbnMessage = "Invalid ISBN format.";
        public const string InvalidPriceMessage = "Price should be greater than 0";
        public const string InvalidQuantityMessage = "Quantity should be greater than -1";
        public const string InvalidBookStatusMessage = "BookStatus must be Created";
        public const string InvalidEmailMessage = "Invalid email format.";
        public const string InvalidTotalMessage = "Total should be greater than 0";
        public const string InvalidPageNumberMessage = "Page number is invalid";
        public const string InvalidPageSizeMessage = "Page size is invalid";

        public const string ContentType = "application/json";
        public const string ApplicationError = "Application Error";
        public const string NotFound = "Not Found";
        public const string InvalidParams = "invalidParams";
        public const string ValidationError = "Validation Error";
        public const string ServerError = "Server Error";
        public const string ExceptionInRequest = "Exception in request";
        public const string CustomerExists = "Customer is registered";
        public const string BookRecordNotFound = "Book record is not found";
        public const string OrderRecordNotFound = "Order record is not found";

        public static readonly HashSet<BookStatus> AllowedBookStatuses = new()
        {
            BookStatus.Created,
        };

        public const string OrderBucket = "order";
        public const string CustomerBucket = "customer";
        public const string BookBucket = "book";
        public const string EmailField = "email";
        public const string CustomerIdField = "customer.id";
        public const string QuantityField = "quantity";
        public const string BookStatusField = "bookStatus";
        public const string LastModifiedDateField = "lastModifiedDate";
        public const string OutOfStockField = "outOfStock";
        public const string OrderDateField = "orderDate";
    }
}

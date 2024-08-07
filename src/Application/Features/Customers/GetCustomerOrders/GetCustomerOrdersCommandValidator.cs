using Application.Constants;

namespace Application.Features.Customers.GetCustomerOrders;

public class GetCustomerOrdersValidator : AbstractValidator<GetCustomerOrdersQuery>
{
    public GetCustomerOrdersValidator()
    {
        RuleFor(p => p.PageNumber)
            .GreaterThan(AppConstants.MinPageNumber)
            .LessThan(AppConstants.MaxPageNumber)
            .WithMessage(AppConstants.InvalidPageNumberMessage);

        RuleFor(p => p.PageSize)
            .GreaterThan(AppConstants.MinPageSize)
            .LessThan(AppConstants.MaxPageSize)
            .WithMessage(AppConstants.InvalidPageSizeMessage);

    }
}

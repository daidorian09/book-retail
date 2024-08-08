using Application.Constants;

namespace Application.Features.Customers.GetCustomerOrders;

public class GetCustomerOrdersQueryValidator : AbstractValidator<GetCustomerOrdersQuery>
{
    public GetCustomerOrdersQueryValidator()
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

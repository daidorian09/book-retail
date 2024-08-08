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

    }
}

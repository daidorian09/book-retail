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

    }
}

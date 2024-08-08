using Application.Constants;

namespace Application.Features.Orders.GetOrdersWithDate;

public class GetOrdersWitDateQueryValidator : AbstractValidator<GetOrdersWithDateQuery>
{
    public GetOrdersWitDateQueryValidator()
    {
        RuleFor(p => p.StartDate)
            .GreaterThan(AppConstants.MinStartDate);

        RuleFor(p => p.EndDate)
            .GreaterThan(AppConstants.MinEndDate);
    }
}

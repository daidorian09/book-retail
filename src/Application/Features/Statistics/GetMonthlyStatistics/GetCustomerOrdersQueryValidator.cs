using Application.Constants;

namespace Application.Features.Statistics.GetMonthlyStatistics;

public class GetMonthlyStatisticsQueryValidator : AbstractValidator<GetMonthlyStatisticsQuery>
{
    public GetMonthlyStatisticsQueryValidator()
    {
        RuleFor(p => p.Year)
            .GreaterThan(AppConstants.MinYear);

    }
}

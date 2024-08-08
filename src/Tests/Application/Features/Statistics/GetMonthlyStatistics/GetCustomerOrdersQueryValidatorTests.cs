using Application.Features.Statistics.GetMonthlyStatistics;
using FluentValidation;
using Tests.Application.Common;

namespace Tests.Application.Features.Statistics.GetMonthlyStatistics
{
    public class GetMonthlyStatisticsQueryValidatorTests : ValidatorTestBase<GetMonthlyStatisticsQuery>
    {
        [Fact]
        public void Year_CannotBeLessThan0()
        {
            Action<GetMonthlyStatisticsQuery> mutation = x => x.Year = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Year);
        }

        protected override GetMonthlyStatisticsQuery CreateValidObject()
        {
            return new GetMonthlyStatisticsQuery
            {
                Year = 2024
            };
        }

        protected override IValidator<GetMonthlyStatisticsQuery> CreateValidator()
        {
            return new GetMonthlyStatisticsQueryValidator();
        }
    }
}

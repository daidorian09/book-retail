using Application.Features.Orders.GetOrdersWithDate;
using FluentValidation;
using Tests.Application.Common;

namespace Tests.Application.Features.Order.GetOrder
{
    public class GetOrdersWithDateQueryValidatorTests : ValidatorTestBase<GetOrdersWithDateQuery>
    {
        [Fact]
        public void StartDate_CannotBeLessThan0()
        {
            Action<GetOrdersWithDateQuery> mutation = x => x.StartDate = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        [Fact]
        public void EndDate_CannotBeLessThan0()
        {
            Action<GetOrdersWithDateQuery> mutation = x => x.EndDate = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.EndDate);
        }

        protected override IValidator<GetOrdersWithDateQuery> CreateValidator()
        {
            return new GetOrdersWitDateQueryValidator();
        }

        protected override GetOrdersWithDateQuery CreateValidObject()
        {
            return new GetOrdersWithDateQuery
            {
                EndDate = 123,
                StartDate = 123
            };
        }
    }
}

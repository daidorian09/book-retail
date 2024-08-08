using Application.Features.Customers.GetCustomerOrders;
using FluentValidation;
using Tests.Application.Common;

namespace Tests.Application.Features.Customers.GetCustomerOrders
{
    public class GetCustomerOrdersQueryValidatorTests : ValidatorTestBase<GetCustomerOrdersQuery>
    {
        [Fact]
        public void PageSize_CannotBeLessThan0()
        {
            Action<GetCustomerOrdersQuery> mutation = x => x.PageSize = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Fact]
        public void PageSize_CannotExceed100()
        {
            Action<GetCustomerOrdersQuery> mutation = x => x.PageSize = 101;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Fact]
        public void PageNumber_CannotBeLessThan0()
        {
            Action<GetCustomerOrdersQuery> mutation = x => x.PageNumber = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }

        [Fact]
        public void PageNumber_CannotExceed100()
        {
            Action<GetCustomerOrdersQuery> mutation = x => x.PageNumber = 101;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }

        protected override GetCustomerOrdersQuery CreateValidObject()
        {
            return new GetCustomerOrdersQuery
            {
                PageSize = 1,
                PageNumber = 2
            };
        }

        protected override IValidator<GetCustomerOrdersQuery> CreateValidator()
        {
            return new GetCustomerOrdersQueryValidator();
        }
    }
}

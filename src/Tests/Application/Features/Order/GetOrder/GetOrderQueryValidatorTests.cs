using Application.Features.Orders.CreateOrder;
using Application.Features.Orders.GetOrder;
using FluentValidation;
using Tests.Application.Common;

namespace Tests.Application.Features.Order.GetOrder
{
    public class GetOrderQueryValidatorTests : ValidatorTestBase<GetOrderQuery>
    {

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void OrderId_CannotBeEmpty(string id)
        {
            Action<GetOrderQuery> mutation = x => x.Id = id;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
        protected override IValidator<GetOrderQuery> CreateValidator()
        {
            return new GetOrderQueryValidator();
        }

        protected override GetOrderQuery CreateValidObject()
        {
            return new GetOrderQuery
            {
                Id = "1"
            };
        }
    }
}

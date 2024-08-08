using Application.Features.Orders.CreateOrder;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Tests.Application.Common;

namespace Tests.Application.Features.Order.CreateOrder
{
    public class CreateOrderCommandValidatorTests : ValidatorTestBase<CreateOrderCommand>
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShipmentAddress_CannotBeEmpty(string shipmentAddress)
        {
            Action<CreateOrderCommand> mutation = x => x.ShipmentAddress = shipmentAddress;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.ShipmentAddress);
        }

        [Fact]
        public void ShipmentAddress_Length_CannotExceed256()
        {
            Action<CreateOrderCommand> mutation = x => x.ShipmentAddress = new string('*', 257);

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.ShipmentAddress);
        }

        [Fact]
        public void Items_Length_CannotBeEmpty()
        {
            Action<CreateOrderCommand> mutation = x => x.Items = new List<BookMetaData>();

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Items);
        }

        [Fact]
        public void Total_CannotBeLessThan0()
        {
            Action<CreateOrderCommand> mutation = x => x.Total = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Total);
        }

        protected override CreateOrderCommand CreateValidObject()
        {
            return new CreateOrderCommand
            {
                Total = 123,
                Items = new List<Domain.Entities.BookMetaData>(),
                OrderStatus = OrderStatus.Created.ToString(),
                PaymentMethod = PaymentMethod.CreditCard.ToString(),
                ShipmentAddress = "test",
                Customer = default,
            };
        }

        protected override IValidator<CreateOrderCommand> CreateValidator()
        {
            return new CreateOrderCommandValidator();
        }
    }
}

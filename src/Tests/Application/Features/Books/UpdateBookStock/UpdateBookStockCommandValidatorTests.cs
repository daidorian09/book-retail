using Application.Features.Books.CreateBook;
using Application.Features.Books.UpdateBookStock;
using FluentValidation;
using Tests.Application.Common;

namespace Tests.Application.Features.Books.UpdateBookStock
{
    public class UpdateBookStockCommandValidatorTests : ValidatorTestBase<UpdateBookStockCommand>
    {
        [Fact]
        public void Quantity_CannotLessThan0()
        {
            Action<UpdateBookStockCommand> mutation = x => x.Quantity = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }


        protected override IValidator<UpdateBookStockCommand> CreateValidator()
        {
            return new UpdateBookStockCommandValidator();

        }

        protected override UpdateBookStockCommand CreateValidObject()
        {
            return new UpdateBookStockCommand
            {
                Id = "1",
                Quantity = 1
            };
        }
    }
}

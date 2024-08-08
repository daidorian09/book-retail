using Application.Features.Customers.CreateCustomer;
using FluentValidation;
using Tests.Application.Common;

namespace Tests.Application.Features.Customers.CreateCustomer
{
    public class CreateCustomerCommandValidatorTests : ValidatorTestBase<CreateCustomerCommand>
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void FirstName_CannotBeEmpty(string firstName)
        {
            Action<CreateCustomerCommand> mutation = x => x.FirstName = firstName;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void FirstName_Length_CannotExceed100()
        {
            Action<CreateCustomerCommand> mutation = x => x.FirstName = new string('*', 101);

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void LastName_Length_CannotExceed100()
        {
            Action<CreateCustomerCommand> mutation = x => x.LastName = new string('*', 101);

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Address_Length_CannotExceed100()
        {
            Action<CreateCustomerCommand> mutation = x => x.Address = new string('*', 101);

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Theory]
        [InlineData("*")]
        [InlineData("test")]
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("fake@")]
        [InlineData("fake~")]
        public void Email_Format_Invalid(string email)
        {
            Action<CreateCustomerCommand> mutation = x => x.Email = email;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void LastName_CannotBeEmpty(string lastName)
        {
            Action<CreateCustomerCommand> mutation = x => x.LastName = lastName;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Email_CannotBeEmpty(string email)
        {
            Action<CreateCustomerCommand> mutation = x => x.Email = email;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Address_CannotBeEmpty(string address)
        {
            Action<CreateCustomerCommand> mutation = x => x.Address = address;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }

        protected override CreateCustomerCommand CreateValidObject()
        {
            return new CreateCustomerCommand
            {
                Address = "test",
                Email = "test@test.com",
                FirstName = "test",
                LastName = "test",
            };
        }

        protected override IValidator<CreateCustomerCommand> CreateValidator()
        {
            return new CreateCustomerCommandValidator();
        }
    }
}

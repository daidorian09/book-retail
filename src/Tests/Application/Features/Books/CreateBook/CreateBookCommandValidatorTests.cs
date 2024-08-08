using Application.Features.Books.CreateBook;
using FluentValidation;
using Tests.Application.Common;

namespace Tests.Application.Features.Books.CreateBook
{
    public class CreateBookCommandValidatorTests : ValidatorTestBase<CreateBookCommand>
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Author_CannotBeEmpty(string author)
        {
            Action<CreateBookCommand> mutation = x => x.Author = author;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Author);
        }

        [Fact]
        public void Author_Length_CannotExceed100()
        {
            Action<CreateBookCommand> mutation = x => x.Author = new string('*', 101);

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Author);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Title_CannotBeEmpty(string title)
        {
            Action<CreateBookCommand> mutation = x => x.Title = title;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Title_Length_CannotExceed100()
        {
            Action<CreateBookCommand> mutation = x => x.Title = new string('*', 101);

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ISBN446877428FCI")]
        [InlineData("I_SB%^N_/(")]
        [InlineData("978-1-12345-909-4 2")]
        public void Isbn_CannotBeInvalidFormat(string isbn)
        {
            Action<CreateBookCommand> mutation = x => x.ISBN = isbn;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.ISBN);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Image_CannotBeEmpty(string image)
        {
            Action<CreateBookCommand> mutation = x => x.Image = image;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Image);
        }

        [Fact]
        public void Image_Length_CannotExceed100()
        {
            Action<CreateBookCommand> mutation = x => x.Image = new string('*', 101);

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Image);
        }

        [Fact]
        public void Price_CannotBeLessThan0()
        {
            Action<CreateBookCommand> mutation = x => x.Price = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Quantity_CannotBeLessThan0()
        {
            Action<CreateBookCommand> mutation = x => x.Quantity = -1;

            var result = Validate(mutation);

            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }


        protected override CreateBookCommand CreateValidObject()
        {
            return new CreateBookCommand
            {
                Title = "Title",
                Quantity = 1,
                Image = "image",
                Price = 100,
                Author = "Author",
                ISBN = "978-1-45678-123-4"
            };
        }

        protected override IValidator<CreateBookCommand> CreateValidator()
        {
            return new CreateBookCommandValidator();
        }
    }
}

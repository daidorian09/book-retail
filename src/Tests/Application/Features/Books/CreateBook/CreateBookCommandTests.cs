using Application.Persistence;
using AutoFixture.AutoMoq;
using AutoFixture;
using Domain.Entities;
using Moq;
using Application.Features.Books.CreateBook;
using Application.Constants;
using Shouldly;

namespace Tests.Application.Features.Books.CreateBook
{
    public class CreateBookCommandTests
    {
        private readonly IFixture _fixture;
        private readonly CreateBookCommandHandler _sut;
        private readonly Mock<IRepository<Book>> _bookRepository;

        public CreateBookCommandTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _bookRepository = _fixture.Freeze<Mock<IRepository<Book>>>();
            _sut = _fixture.Create<CreateBookCommandHandler>();
        }

        [Fact]
        public async Task CreateBook_ReturnsSucceess()
        {
            var command = new CreateBookCommand
            {
                Author = "test",
                Image = "test.jpeg",
                ISBN = "978-1-45678-123-4",
                Price = 100,
                Quantity = 1,
                Title = "test"
            };

            _bookRepository.Setup(c => c.CreateAsync(AppConstants.BookBucket, It.IsAny<string>(), It.IsAny<Book>())).ReturnsAsync(It.IsAny<string>).Verifiable();

            var result = await _sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            _bookRepository.Verify(c => c.CreateAsync(AppConstants.BookBucket, It.IsAny<string>(), It.IsAny<Book>()), Times.Once);
        }
    }
}

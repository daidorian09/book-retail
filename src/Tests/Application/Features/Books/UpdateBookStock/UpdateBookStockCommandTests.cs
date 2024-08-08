using Application.Constants;
using Application.Exceptions;
using Application.Features.Books.UpdateBookStock;
using Application.Persistence;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Moq;
using Shouldly;

namespace Tests.Application.Features.Books.CreateBook
{
    public class UpdateBookStockCommandTests
    {
        private readonly IFixture _fixture;
        private readonly UpdateBookStockCommandHandler _sut;
        private readonly Mock<IRepository<Book>> _bookRepository;

        public UpdateBookStockCommandTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _bookRepository = _fixture.Freeze<Mock<IRepository<Book>>>();
            _sut = _fixture.Create<UpdateBookStockCommandHandler>();
        }

        [Fact]
        public async Task UpdateBookStock_ReturnsSucceess()
        {
            var id = Guid.NewGuid();
            var command = new UpdateBookStockCommand
            {
                Id = id.ToString(),
                Quantity = 5
            };


            var book = new Book()
            {
                Id = id,
                Quantity = 6,
            };

            ulong cas = 9497804238020438090;

            _bookRepository.Setup(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, command.Id)).ReturnsAsync((book, cas)).Verifiable();
            _bookRepository.Setup(c => c.PartialUpdateAsync(AppConstants.BookBucket, command.Id, It.IsAny<Dictionary<string, object>>(), cas)).ReturnsAsync(true).Verifiable();

            var result = await _sut.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeTrue();
            _bookRepository.Verify(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, command.Id), Times.Once);
            _bookRepository.Verify(c => c.PartialUpdateAsync(AppConstants.BookBucket, command.Id, It.IsAny<Dictionary<string, object>>(), cas), Times.Once);
        }

        [Fact]
        public async Task UpdateBookStock_WithNotExistingBook_ThrowsException()
        {
            var id = Guid.NewGuid();
            var command = new UpdateBookStockCommand
            {
                Id = id.ToString(),
                Quantity = 5
            };


            var book = new Book()
            {
                Id = id,
                Quantity = 6,
            };

            ulong cas = 9497804238020438090;

            _bookRepository.Setup(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, command.Id)).ReturnsAsync((default, cas)).Verifiable();

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _sut.Handle(command, CancellationToken.None);
            });

            exception.Message.ShouldBe(AppConstants.BookRecordNotFound);
            _bookRepository.Verify(c => c.GetByIdWithCasAsync(AppConstants.BookBucket, command.Id), Times.Once);
            _bookRepository.Verify(c => c.PartialUpdateAsync(AppConstants.BookBucket, command.Id, It.IsAny<Dictionary<string, object>>(), cas), Times.Never);
        }
    }
}
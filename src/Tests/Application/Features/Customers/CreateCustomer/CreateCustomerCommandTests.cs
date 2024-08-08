using Application.Constants;
using Application.Exceptions;
using Application.Features.Customers.CreateCustomer;
using Application.Persistence;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Moq;
using Shouldly;

namespace Tests.Application.Features.Customers.CreateCustomer;

public class CreateCustomerCommandTests
{
    private readonly IFixture _fixture;
    private readonly CreateCustomerCommandHandler _sut;
    private readonly Mock<IRepository<Customer>> _customerRepository;

    public CreateCustomerCommandTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _customerRepository = _fixture.Freeze<Mock<IRepository<Customer>>>();
        _sut = _fixture.Create<CreateCustomerCommandHandler>();
    }

    [Fact]
    public async Task CreateCustomer_WithNonExistingCustomer_ReturnsSucceess()
    {
        var command = new CreateCustomerCommand
        {
            Email = "test@test.com",
            Address = "address",
            FirstName = "test",
            LastName = "Test"
        };


        _customerRepository.Setup(c => c.ExistsAsync(AppConstants.CustomerBucket, AppConstants.EmailField, command.Email)).ReturnsAsync(false).Verifiable();
        _customerRepository.Setup(c => c.CreateAsync(AppConstants.CustomerBucket, It.IsAny<string>(), It.IsAny<Customer>())).ReturnsAsync(It.IsAny<string>).Verifiable();

        var result = await _sut.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        _customerRepository.Verify(c => c.ExistsAsync(AppConstants.CustomerBucket, AppConstants.EmailField, command.Email), Times.Once);
        _customerRepository.Verify(c => c.CreateAsync(AppConstants.CustomerBucket, It.IsAny<string>(), It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task CreateCustomer_WithExistingCustomer_ThrowsException()
    {
        var command = new CreateCustomerCommand
        {
            Email = "test@test.com",
            Address = "address",
            FirstName = "test",
            LastName = "Test"
        };


        _customerRepository.Setup(c => c.ExistsAsync(AppConstants.CustomerBucket, AppConstants.EmailField, command.Email)).ReturnsAsync(true).Verifiable();

        var exception = await Assert.ThrowsAsync<CustomerExistsException>(async () =>
        {
            await _sut.Handle(command, CancellationToken.None);
        });

        exception.Message.ShouldBe(AppConstants.CustomerExists);
        _customerRepository.Verify(c => c.ExistsAsync(AppConstants.CustomerBucket, AppConstants.EmailField, command.Email), Times.Once);
        _customerRepository.Verify(c => c.CreateAsync(AppConstants.CustomerBucket, It.IsAny<string>(), It.IsAny<Customer>()), Times.Never);
    }
}

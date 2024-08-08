using Application.Constants;
using Application.Exceptions;
using Application.Persistence;
using Domain.Entities;

namespace Application.Features.Customers.CreateCustomer;

public class CreateCustomerCommand : IRequest<Result<CreateCustomerCommandResponse>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CreateCustomerCommandResponse>>
{
    private readonly IRepository<Customer> _customerRepository;

    public CreateCustomerCommandHandler(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CreateCustomerCommandResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var existsResult = await _customerRepository.ExistsAsync(AppConstants.CustomerBucket, AppConstants.EmailField, request.Email);

        if(existsResult)
        {
            throw new CustomerExistsException(AppConstants.CustomerExists);
        }

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            Email = request.Email,
            CreatedDate = (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds
        };

        await _customerRepository.CreateAsync(AppConstants.CustomerBucket, customer.Id.ToString(), customer);

        return Result.Ok(new CreateCustomerCommandResponse { Id = customer.Id });
    }
}
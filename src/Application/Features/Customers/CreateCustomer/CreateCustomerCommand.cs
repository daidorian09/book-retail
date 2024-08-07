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
    public CreateCustomerCommandHandler()
    {
        
    }

    public async Task<Result<CreateCustomerCommandResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            Email = request.Email,
            CreatedDate = DateTimeOffset.UtcNow
        };

        return Result.Ok(new CreateCustomerCommandResponse { Id = customer.Id });
    }
}
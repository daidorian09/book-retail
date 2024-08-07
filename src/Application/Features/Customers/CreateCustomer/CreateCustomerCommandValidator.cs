namespace Application.Features.Customers.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(p => p.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(p => p.Address)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(p => p.Email)
           .NotEmpty()
           .EmailAddress()
           .WithMessage("Invalid email format.");

    }
}

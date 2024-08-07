using Application.Constants;

namespace Application.Features.Customers.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty()
            .MaximumLength(AppConstants.MaxTextLength);

        RuleFor(p => p.LastName)
            .NotEmpty()
            .MaximumLength(AppConstants.MaxTextLength);

        RuleFor(p => p.Address)
            .NotEmpty()
            .MaximumLength(AppConstants.MaxTextLength);

        RuleFor(p => p.Email)
           .NotEmpty()
           .EmailAddress()
           .WithMessage(AppConstants.InvalidEmailMessage);

    }
}

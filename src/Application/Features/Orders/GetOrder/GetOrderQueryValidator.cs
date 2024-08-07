using Application.Features.Orders.CreateOrder;

namespace Application.Features.Orders.GetOrder;

public class GetOrderQueryValidator : AbstractValidator<GetOrderQuery>
{
    public GetOrderQueryValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty();
    }
}
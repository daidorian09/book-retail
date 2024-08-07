using Application.Features.Customers.CreateCustomer;
using Infrastructure.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Create))]
    [HttpPost(Name = "CreateCustomer")]
    public async Task<ActionResult> Create(CreateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }
}
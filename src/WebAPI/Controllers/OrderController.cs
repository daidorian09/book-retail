using Application.Features.Orders.CreateOrder;
using Infrastructure.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Create))]
    [HttpPost(Name = "CreateOrder")]
    public async Task<ActionResult> Create(CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }
}
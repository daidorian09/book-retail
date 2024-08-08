using Application.Features.Orders.CreateOrder;
using Application.Features.Orders.GetOrdersWithDate;
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

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Get))]
    [HttpGet("{id:Guid}", Name = "GetOrder")]
    public async Task<ActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new GetOrderQuery { Id = id.ToString() });
        return result.ToHttpResponse();
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.List))]
    [HttpGet("list/{startDate:int}/{endDate:int}", Name = "GetOrdersWithDate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetOrdersWithDateQueryResponse>))]
    public async Task<ActionResult> GetOrdersWithDate(long startDate, long endDate)
    {
        var result = await _mediator.Send(new GetOrdersWithDateQuery { StartDate = startDate, EndDate = endDate });
        return result.ToHttpResponse();
    }
}
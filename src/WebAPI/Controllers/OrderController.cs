using Application.Features.Orders.CreateOrder;
using Application.Features.Orders.GetOrdersWithDate;
using Infrastructure.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> Create(CreateOrderCommand command, [FromHeader(Name = "request-owner-id")] string requestOwnerId,
        [FromHeader(Name = "role")] string role)
    {
        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Get))]
    [HttpGet("{id:Guid}", Name = "GetOrder")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> Get(Guid id, [FromHeader(Name = "request-owner-id")] string requestOwnerId, [FromHeader(Name = "role")] string role)
    {
        var result = await _mediator.Send(new GetOrderQuery { Id = id.ToString() });
        return result.ToHttpResponse();
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.List))]
    [HttpGet("list/{startDate:int}/{endDate:int}", Name = "GetOrdersWithDate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetOrdersWithDateQueryResponse>))]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> GetOrdersWithDate(long startDate, long endDate, [FromHeader(Name = "request-owner-id")] string requestOwnerId,
        [FromHeader(Name = "role")] string role)
    {
        var result = await _mediator.Send(new GetOrdersWithDateQuery { StartDate = startDate, EndDate = endDate });
        return result.ToHttpResponse();
    }
}
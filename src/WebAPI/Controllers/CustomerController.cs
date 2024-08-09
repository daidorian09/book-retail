using Application.Features.Customers.CreateCustomer;
using Application.Features.Customers.GetCustomerOrders;
using Application.Models;
using CleanArchitecture.Application.Features.Products.GetPagedProducts;
using Infrastructure.Controllers;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> Create(CreateCustomerCommand command, [FromHeader(Name = "request-owner-id")] string requestOwnerId, [FromHeader(Name = "role")] string role)
    {
        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.List))]
    [HttpGet("list/{pageNumber:int}/{pageSize:int}", Name = "GetCustomerOrdersWithPagination")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<GetCustomerOrdersQueryResponse>))]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> GetPaginatedCustomerOrders(int pageNumber, int pageSize, [FromHeader(Name = "request-owner-id")] string requestOwnerId,
        [FromHeader(Name = "role")] string role)
    {
        var result = await _mediator.Send(new GetCustomerOrdersQuery { PageNumber = pageNumber, PageSize = pageSize, CustomerId = User.GetUserId() });
        return result.ToHttpResponse();
    }
}
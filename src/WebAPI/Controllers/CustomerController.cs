using Application.Features.Customers.CreateCustomer;
using Application.Features.Customers.GetCustomerOrders;
using Application.Models;
using CleanArchitecture.Application.Features.Products.GetPagedProducts;
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

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.List))]
    [HttpGet("paginated/{pageNumber:int}/{pageSize:int}", Name = "GetCustomerOrdersWithPagination")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<GetCustomerOrdersQueryResponse>))]
    public async Task<ActionResult> GetPagedProducts(int pageNumber, int pageSize)
    {
        var result = await _mediator.Send(new GetCustomerOrdersQuery { PageNumber = pageNumber, PageSize = pageSize });
        return result.ToHttpResponse();
    }
}
using Application.Features.Books.CreateBook;
using Application.Features.Books.UpdateBookStock;
using Infrastructure.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Create))]
    [HttpPost(Name = "CreateBook")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> Create(CreateBookCommand command, [FromHeader(Name = "request-owner-id")] string requestOwnerId, [FromHeader(Name = "role")] string role)
    {
        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Update))]
    [HttpPatch(Name = "UpdateBookStock")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult> Update(UpdateBookStockCommand command, [FromHeader(Name = "request-owner-id")] string requestOwnerId,
        [FromHeader(Name = "role")] string role)
    {
        var result = await _mediator.Send(command);
        return result.ToCreatedNoContentResponse();
    }
}
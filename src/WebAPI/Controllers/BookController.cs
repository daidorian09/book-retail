﻿using Application.Features.Books.CreateBook;
using Application.Features.Books.UpdateBookStock;
using Infrastructure.Controllers;
using MediatR;
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
    public async Task<ActionResult> Create(CreateBookCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToCreatedHttpResponse();
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Update))]
    [HttpPatch(Name = "UpdateBookStock")]
    public async Task<ActionResult> Update(UpdateBookStockCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToCreatedNoContentResponse();
    }
}
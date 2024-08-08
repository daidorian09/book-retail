﻿using Application.Features.Orders.GetOrdersWithDate;
using Application.Features.Statistics.GetMonthlyStatistics;
using Infrastructure.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class StatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.List))]
    [HttpGet("monthly-statistics/{year:int}", Name = "GetMonthlyStatistics")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetMonthlyStatisticsQueryResponse>))]
    public async Task<ActionResult> GetMonthlyStatistics(int year)
    {
        var result = await _mediator.Send(new GetMonthlyStatisticsQuery { Year = year });
        return result.ToHttpResponse();
    }
}
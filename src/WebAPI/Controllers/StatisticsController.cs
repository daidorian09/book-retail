using Application.Features.Statistics.GetMonthlyStatistics;
using Infrastructure.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> GetMonthlyStatistics(int year, [FromHeader(Name = "request-owner-id")] string requestOwnerId,
        [FromHeader(Name = "role")] string role)
    {
        var result = await _mediator.Send(new GetMonthlyStatisticsQuery { Year = year });
        return result.ToHttpResponse();
    }
}
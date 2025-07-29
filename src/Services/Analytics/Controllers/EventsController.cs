using Microsoft.AspNetCore.Mvc;
using MediatR;
using Analytics.Dtos;
using Analytics.Application.Events.Queries;
using Analytics.Application.Events.Commands;
using Analytics.Services;

namespace Analytics.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEventService _service;  // Direct for metrics

    public EventsController(IMediator mediator, IEventService service)
    {
        _mediator = mediator;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var result = await _mediator.Send(new GetEventsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var result = await _mediator.Send(new GetEventByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] EventDto dto)
    {
        var created = await _mediator.Send(new CreateEventCommand(dto));
        return CreatedAtAction(nameof(GetEvent), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventDto dto)
    {
        await _mediator.Send(new UpdateEventCommand(id, dto));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        await _mediator.Send(new DeleteEventCommand(id));
        return NoContent();
    }

    [HttpGet("metrics/{campaignId}")]
    public async Task<IActionResult> GetMetrics(Guid campaignId)
    {
        var metrics = await _service.CalculateMetricsAsync(campaignId);
        return Ok(metrics);
    }
}
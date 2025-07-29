using Microsoft.AspNetCore.Mvc;
using UserSegmentation.Dtos;
using MediatR;
using UserSegmentation.Application.Segments.Queries;
using UserSegmentation.Application.Segments.Commands;

namespace UserSegmentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SegmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SegmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetSegments([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var segments = await _mediator.Send(new GetSegmentsQuery(page, pageSize));
        return Ok(segments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSegment(Guid id)
    {
        var segment = await _mediator.Send(new GetSegmentByIdQuery(id));
        if (segment == null) return NotFound();
        return Ok(segment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSegment([FromBody] SegmentDto dto)
    {
        var created = await _mediator.Send(new CreateSegmentCommand(dto));
        return CreatedAtAction(nameof(GetSegment), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSegment(Guid id, [FromBody] SegmentDto dto)
    {
        await _mediator.Send(new UpdateSegmentCommand(id, dto));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSegment(Guid id)
    {
        await _mediator.Send(new DeleteSegmentCommand(id));
        return NoContent();
    }

    [HttpPost("{id}/users")]
    public async Task<IActionResult> GetSegmentUsers(Guid id, [FromBody] SegmentCriteriaDto criteria, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var users = await _mediator.Send(new GetSegmentUsersQuery(id, criteria, page, pageSize));
        return Ok(users);
    }

    [HttpGet("{id}/export")]
    public async Task<IActionResult> ExportSegment(Guid id, [FromQuery] string format = "json")
    {
        // Для экспорта используем пустые критерии (или можно добавить в query/body)
        var criteria = new SegmentCriteriaDto();
        var data = await _mediator.Send(new ExportSegmentQuery(id, criteria, format));
        if (format.ToLower() == "csv")
            return File(System.Text.Encoding.UTF8.GetBytes(data), "text/csv", $"segment_{id}.csv");
        return File(System.Text.Encoding.UTF8.GetBytes(data), "application/json", $"segment_{id}.json");
    }
} 
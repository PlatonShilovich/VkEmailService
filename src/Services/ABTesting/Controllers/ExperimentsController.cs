using Microsoft.AspNetCore.Mvc;
using MediatR;
using ABTesting.Dtos;
using ABTesting.Application.Experiments.Queries;
using ABTesting.Application.Experiments.Commands;

namespace ABTesting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExperimentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ExperimentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetExperiments()
    {
        var result = await _mediator.Send(new GetExperimentsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExperiment(Guid id)
    {
        var result = await _mediator.Send(new GetExperimentByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExperiment([FromBody] ExperimentDto dto)
    {
        var created = await _mediator.Send(new CreateExperimentCommand(dto));
        return CreatedAtAction(nameof(GetExperiment), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExperiment(Guid id, [FromBody] ExperimentDto dto)
    {
        await _mediator.Send(new UpdateExperimentCommand(id, dto));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExperiment(Guid id)
    {
        await _mediator.Send(new DeleteExperimentCommand(id));
        return NoContent();
    }

    [HttpPut("{id}/start")]
    public async Task<IActionResult> StartExperiment(Guid id)
    {
        await _mediator.Send(new StartExperimentCommand(id));
        return NoContent();
    }

    [HttpPut("{id}/stop")]
    public async Task<IActionResult> StopExperiment(Guid id)
    {
        await _mediator.Send(new StopExperimentCommand(id));
        return NoContent();
    }

    [HttpPost("{id}/assign-user")]
    public async Task<IActionResult> AssignUser(Guid id, [FromBody] Guid userId)
    {
        var assignment = await _mediator.Send(new AssignUserCommand(id, userId));
        return Ok(assignment);
    }

    [HttpGet("{id}/results")]
    public async Task<IActionResult> GetResults(Guid id)
    {
        var results = await _mediator.Send(new GetExperimentResultsQuery(id));
        return Ok(results);
    }
} 
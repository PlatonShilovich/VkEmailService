using Microsoft.AspNetCore.Mvc;
using MediatR;
using EmailCampaign.Dtos;
using EmailCampaign.Application.Campaigns.Queries;
using EmailCampaign.Application.Campaigns.Commands;

namespace EmailCampaign.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
    private readonly IMediator _mediator;
    public CampaignsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetCampaigns()
    {
        var result = await _mediator.Send(new GetCampaignsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCampaign(Guid id)
    {
        var result = await _mediator.Send(new GetCampaignByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCampaign([FromBody] CampaignDto dto)
    {
        var created = await _mediator.Send(new CreateCampaignCommand(dto));
        return CreatedAtAction(nameof(GetCampaign), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCampaign(Guid id, [FromBody] CampaignDto dto)
    {
        await _mediator.Send(new UpdateCampaignCommand(id, dto));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCampaign(Guid id)
    {
        await _mediator.Send(new DeleteCampaignCommand(id));
        return NoContent();
    }
} 
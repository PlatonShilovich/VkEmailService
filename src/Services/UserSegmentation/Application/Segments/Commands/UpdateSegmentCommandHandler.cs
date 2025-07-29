using MediatR;
using UserSegmentation.Dtos;
using UserSegmentation.Services;

namespace UserSegmentation.Application.Segments.Commands;

public class UpdateSegmentCommandHandler : IRequestHandler<UpdateSegmentCommand>
{
    private readonly ISegmentService _service;
    public UpdateSegmentCommandHandler(ISegmentService service) => _service = service;
    public async Task Handle(UpdateSegmentCommand request, CancellationToken cancellationToken)
        => await _service.UpdateSegmentAsync(request.Id, request.Dto);
} 
using MediatR;
using UserSegmentation.Dtos;
using UserSegmentation.Services;

namespace UserSegmentation.Application.Segments.Commands;

public class CreateSegmentCommandHandler : IRequestHandler<CreateSegmentCommand, SegmentDto>
{
    private readonly ISegmentService _service;
    public CreateSegmentCommandHandler(ISegmentService service) => _service = service;
    public async Task<SegmentDto> Handle(CreateSegmentCommand request, CancellationToken cancellationToken)
        => await _service.CreateSegmentAsync(request.Dto);
} 
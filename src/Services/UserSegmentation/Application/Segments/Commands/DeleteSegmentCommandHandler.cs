using MediatR;
using UserSegmentation.Services;

namespace UserSegmentation.Application.Segments.Commands;

public class DeleteSegmentCommandHandler : IRequestHandler<DeleteSegmentCommand>
{
    private readonly ISegmentService _service;
    public DeleteSegmentCommandHandler(ISegmentService service) => _service = service;
    public async Task Handle(DeleteSegmentCommand request, CancellationToken cancellationToken)
        => await _service.DeleteSegmentAsync(request.Id);
} 
using FluentValidation;
using UserSegmentation.Dtos;

namespace UserSegmentation.Validators;

public class SegmentDtoValidator : AbstractValidator<SegmentDto>
{
    public SegmentDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Criteria).NotEmpty();
    }
} 
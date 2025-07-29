using FluentValidation;
using UserSegmentation.Dtos;

namespace UserSegmentation.Validators;

public class SegmentCriteriaDtoValidator : AbstractValidator<SegmentCriteriaDto>
{
    public SegmentCriteriaDtoValidator()
    {
        RuleFor(x => x.AgeRange)
            .Must(r => !r.HasValue || (r.Value.Min >= 0 && r.Value.Max >= r.Value.Min))
            .WithMessage("Некорректный диапазон возраста");
        RuleFor(x => x.Genders)
            .Must(g => g == null || g.All(s => !string.IsNullOrWhiteSpace(s)))
            .WithMessage("Пустое значение пола недопустимо");
        RuleFor(x => x.Locations)
            .Must(l => l == null || l.All(s => !string.IsNullOrWhiteSpace(s)))
            .WithMessage("Пустое значение локации недопустимо");
        RuleFor(x => x.ActivityPeriod)
            .Must(p => !p.HasValue || p.Value.From <= p.Value.To)
            .WithMessage("Некорректный период активности");
    }
} 
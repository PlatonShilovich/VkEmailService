namespace UserSegmentation.Dtos;

public class SegmentCriteriaDto
{
    public (int Min, int Max)? AgeRange { get; set; }
    public List<string>? Genders { get; set; }
    public List<string>? Locations { get; set; }
    public (DateTime From, DateTime To)? ActivityPeriod { get; set; }
} 
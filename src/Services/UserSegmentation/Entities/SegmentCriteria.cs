namespace UserSegmentation.Entities;

public class SegmentCriteria
{
    public (int Min, int Max)? AgeRange { get; set; }
    public List<string>? Genders { get; set; }
    public List<string>? Locations { get; set; }
    public (DateTime From, DateTime To)? ActivityPeriod { get; set; }
} 
namespace IdCardChecker.Models;

public record IdCardRecord(string Id, string Name, string Sex)
{
    public string? Error { get; set; }
}

public record CandidateInfo(string Name, string CandidateNumber, string DeskNumber, string TestDate);

namespace DataStorage.HelperClasses;

public class ChallengeProgress
{
    public int Done { get; set; }
    public int Total { get; set; }
    public bool Success => Done >= Total;
    public IEnumerable<string?> AllPartialProgresses { get; set; }
}
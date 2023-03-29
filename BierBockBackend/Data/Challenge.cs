using System.ComponentModel.DataAnnotations;

namespace BierBockBackend.Data;

public class Challenge
{
    [Key] public int Id { get; set; }

    public List<PartialChallenge> PartialChallenges { get; set; } = new List<PartialChallenge>();

    public DateTime EndDate { get; set; }
}
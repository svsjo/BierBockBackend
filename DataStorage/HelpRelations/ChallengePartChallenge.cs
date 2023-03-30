using BierBockBackend.Data;
using System.ComponentModel.DataAnnotations;

namespace DataStorage.HelpRelations;

public class ChallengePartChallenge
{
    [Key] public int Id { get; set; }
    public int ChallengeId { get; set; }
    public int ChallengePartId { get; set; }
    public virtual Challenge Challenge { get; set; }
    public virtual ChallengePart ChallengePart { get; set; }
}
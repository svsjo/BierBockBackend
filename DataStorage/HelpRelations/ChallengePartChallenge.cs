using BierBockBackend.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DataStorage.HelpRelations;

public class ChallengePartChallenge
{
    [Key] public int Id { get; set; }

    [JsonIgnore]
    [ForeignKey("ChallengeId")]
    public virtual Challenge Challenge { get; set; }

    [ForeignKey("ChallengePartId")]
    public virtual ChallengePart ChallengePart { get; set; }
}
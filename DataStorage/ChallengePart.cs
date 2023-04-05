using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataStorage.HelpRelations;
using System.Text.Json.Serialization;

namespace BierBockBackend.Data;

public class ChallengePart
{
    [Key] public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    [JsonIgnore]
    public virtual ICollection<ChallengePartChallenge> Challenges { get; set; }


    [ForeignKey("BeerId")]
    public virtual Product Beer { get; set; }
}
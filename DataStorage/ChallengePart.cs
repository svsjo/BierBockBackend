using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataStorage.HelpRelations;

namespace BierBockBackend.Data;

public class ChallengePart
{
    [Key] public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public string BeerCode { get; }

    public int Quantity { get; set; }

    public virtual ICollection<ChallengePartChallenge> Challenges { get; set; }
}
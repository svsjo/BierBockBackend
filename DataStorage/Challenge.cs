using System.ComponentModel.DataAnnotations;
using DataStorage.HelpRelations;

namespace BierBockBackend.Data;

public class Challenge
{
    [Key] public int Id { get; set; }

    public int PossiblePoints { get; set; }

    
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<ChallengePart> PartialChallenges { get; set; }

    public virtual List<ChallengeUser> User { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive => DateTime.Now < EndDate;
}
using System.ComponentModel.DataAnnotations;

namespace BierBockBackend.Data;

public class Challenge
{
    [Key] public int Id { get; set; }

    public int PossiblePoints { get; set; }

    public string Description { get; set; } = string.Empty;

    public virtual IQueryable<ChallengePart> PartialChallenges { get; set; }

    public virtual IQueryable<User> User { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive => DateTime.Now < EndDate;
}
#region

using System.ComponentModel.DataAnnotations;
using DataStorage.HelpRelations;
using System.Text.Json.Serialization;

#endregion

namespace BierBockBackend.Data;

public class Challenge
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    public int PossiblePoints { get; set; }

    public string Description { get; set; } = string.Empty;

    // TODO: Challengetypen definieren!

    [JsonIgnore]
    public virtual ICollection<ChallengeUser> Users { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime EndDate { get; set; }

    public bool IsActive => DateTime.Now < EndDate;
}
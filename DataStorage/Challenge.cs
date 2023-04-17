#region

using System.ComponentModel.DataAnnotations;
using DataStorage.HelpRelations;
using System.Text.Json.Serialization;
using DataStorage.HelperClasses;

#endregion

namespace BierBockBackend.Data;

public abstract class Challenge
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    public int PossiblePoints { get; set; } = 0;

    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime EndDate { get; set; } = DateTime.Now.AddDays(7);

    public bool IsActive => DateTime.Now < EndDate;

    public abstract ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions);
}
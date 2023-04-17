#region

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataStorage;
using DataStorage.HelperClasses;

#endregion

namespace DataStorage;

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

    public int NeededQuantity { get; set; } = 0;

    public abstract ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions);
}
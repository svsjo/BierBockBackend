#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#endregion

namespace DataStorage;

public class User
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string VorName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;

    public int Points { get; set; } = 0;

    [JsonIgnore]
    public ICollection<Challenge> UserChallenges { get; set; } = new List<Challenge>();

    [JsonIgnore]
    public virtual ICollection<DrinkAction> AllDrinkingActions { get; set; }

    public Coordinate Location { get; set; }
    public string Wohnort { get; set; } = string.Empty;

    [JsonIgnore]
    [ForeignKey("BeerId")]
    public virtual Product FavouriteBeer { get; set; }
}
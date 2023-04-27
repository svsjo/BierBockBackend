#region

using DataStorage.HelperClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#endregion

namespace DataStorage;

public class User
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string VorName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public string EmailToken { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(32)]
    public byte[] PasswordSalt { get; set; } = new byte[32];

    public string Email { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;


    public bool AccountLocked { get; set; } = false;

    public int Points { get; set; } = 0;

    [JsonIgnore]
    public virtual ICollection<DrinkAction> AllDrinkingActions { get; set; }

    public virtual Coordinate Location { get; set; }
    public string Wohnort { get; set; } = string.Empty;

    public bool IsAdmin { get; set; } = false;

    [JsonIgnore]
    [ForeignKey("BeerId")]
    public virtual Product FavouriteBeer { get; set; }

    public bool EmailConfirmed { get; set; } = false;
}
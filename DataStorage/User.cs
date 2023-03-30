#region

using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using DataStorage.HelpRelations;
using GeoCoordinatePortable;

#endregion

namespace BierBockBackend.Data;

public class User
{
    [Key] public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FavouriteBeerCode { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public int Points { get; set; } = 0;

    public virtual ICollection<ChallengeUser> UserChallenges { get; set; }

    public virtual ICollection<DrinkAction> AllDrinkingActions { get; set; }

    public GeoCoordinate GetCoordinate => new GeoCoordinate(this.Latitude, this.Longitude, this.Altitude);

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
}
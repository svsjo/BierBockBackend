#region

using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using DataStorage.HelpRelations;

#endregion

namespace BierBockBackend.Data;

public class User
{
    [Key] public int Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public int Points { get; set; } = 0;

    public virtual ICollection<ChallengeUser> UserChallenges { get; set; }

    public virtual ICollection<DrinkAction> AllDrinkingActions { get; set; }

    public Coordinate Location { get; set; }


    /* Fremdschlüssel von FavouriteBeer */
    public int BeerId { get; set; }

    public virtual Product FavouriteBeer { get; set; }
}
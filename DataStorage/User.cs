using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataStorage.HelpRelations;

namespace BierBockBackend.Data;

public class User
{
    [Key] public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public int Points { get; set; } = 0;

    public  ICollection<ChallengeUser> UserChallenges { get; set; }

    public virtual IQueryable<DrinkAction> AllTrinkingActions { get; set; }
}
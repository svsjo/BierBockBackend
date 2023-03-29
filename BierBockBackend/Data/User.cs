using System.ComponentModel.DataAnnotations;

namespace BierBockBackend.Data;

public class User
{
    [Key] public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int Points { get; set; } = 0;

    public List<Challenge> ChallengeList { get; set; } = new List<Challenge>();

    public List<DrinkAction> AllTrinkingActions { get; set; } = new List<DrinkAction>();
}
using System.Drawing;

namespace DataStorage.HelperClasses;

public class SameBeerChallenge : Challenge
{
    public string BeerCode { get; set; } = string.Empty;

    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        int done;

        var drunkBeers = drinkActions.Select(x => x.Product);

        if (!string.IsNullOrEmpty(BeerCode))
        {
            done = drunkBeers.Where(x => x.Code == BeerCode)?.Count() ?? 0;
        }
        else /* Wenn nicht gesetzt, kann es jedes beliebige Bier sein */
        {
            var beerGroups = drunkBeers.GroupBy(x => x.Code);
            done = beerGroups.Select(x => x.Count()).Max();
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = this.NeededQuantity
        };
    }
}
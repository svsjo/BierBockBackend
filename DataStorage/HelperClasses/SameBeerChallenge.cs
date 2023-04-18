using System.Drawing;

namespace DataStorage.HelperClasses;

public class SameBeerChallenge : IChallengeValidator
{
    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString, int neededQuantity)
    {
        int done;

        var drunkBeers = drinkActions.Select(x => x.Product);

        if (!string.IsNullOrEmpty(searchString))
        {
            done = drunkBeers.Where(x => x.Code == searchString)?.Count() ?? 0;
        }
        else /* Wenn nicht gesetzt, kann es jedes beliebige Bier sein */
        {
            var beerGroups = drunkBeers.GroupBy(x => x.Code);
            done = beerGroups.Select(x => x.Count()).Max();
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity
        };
    }
}
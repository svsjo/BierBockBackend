

namespace DataStorage.HelperClasses;

public class SameBrandChallenge : IChallengeValidator
{
    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString, int neededQuantity)
    {
        int done;

        var drunkBeers = drinkActions.Select(x => x.Product);

        if (!string.IsNullOrEmpty(searchString))
        {
            done = drunkBeers.Where(x => x.Brands == searchString)?.Count() ?? 0;
        }
        else /* Wenn nicht gesetzt, kann es jede beliebige Marke sein */
        {
            var brandGroups = drunkBeers.GroupBy(x => x.Brands);
            done = brandGroups.Select(x => x.Count()).Max();
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity
        };
    }
}
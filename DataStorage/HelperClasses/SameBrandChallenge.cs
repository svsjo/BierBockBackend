namespace DataStorage.HelperClasses;

public class SameBrandChallenge : IChallengeValidator
{
    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString, int neededQuantity)
    {
        int done;

        var drunkBeers = drinkActions.Select(x => x.Product);

        IEnumerable<string?> partialProgress;

        if (!string.IsNullOrEmpty(searchString))
        {
            var relatedBeer = drunkBeers.Where(x => x.Brands == searchString).ToList();
            done = relatedBeer?.Count() ?? 0;
            partialProgress = relatedBeer?.Select(x => x.ProductName) ?? new List<string>();
        }
        else /* Wenn nicht gesetzt, kann es jede beliebige Marke sein */
        {
            var brandGroups = drunkBeers.GroupBy(x => x.Brands).ToList();
            done = brandGroups.Select(x => x.Count()).Max();
            partialProgress = brandGroups.First(x => x.Count() == done).Select(x => x.ProductName);
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity,
            AllPartialProgresses = partialProgress
        };
    }
}
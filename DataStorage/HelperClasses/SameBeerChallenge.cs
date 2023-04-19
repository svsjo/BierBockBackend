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
            var beerGroups = drunkBeers.GroupBy(x => x.Code).ToList();
            done = beerGroups.Select(x => x.Count()).Max();
            searchString = beerGroups.First(x => x.Count() == done).First().ProductName;
        }

        var partialProgress = new List<string?>();
        for (int i = 0; i < done; i++)
        {
            partialProgress.Add(searchString);
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity,
            AllPartialProgresses = partialProgress
        };
    }
}
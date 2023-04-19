using System.Runtime.InteropServices;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace DataStorage.HelperClasses;

public class SameSizeChallenge : IChallengeValidator
{
    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString, int neededQuantity)
    {
        var unitNormalizer = new UnitNormalizer();

        int done;

        var drunkBeers = drinkActions.Select(x => x.Product);

        IEnumerable<string?> partialProgress;

        if (!string.IsNullOrEmpty(searchString))
        {
            var relatedBeer = drunkBeers.Where(x => unitNormalizer.NormalizeQuantity(x.Quantity) == searchString).ToList();
            done = relatedBeer?.Count() ?? 0;
            partialProgress = relatedBeer?.Select(x => x.ProductName) ?? new List<string>();
        }
        else /* Wenn nicht gesetzt, kann es jede beliebige Größe sein */
        {
            var sizeGroups = drunkBeers.GroupBy(x => unitNormalizer.NormalizeQuantity(x.Quantity)).ToList();
            done = sizeGroups.Select(x => x.Count()).Max();
            partialProgress = sizeGroups.First(x => x.Count() == done).Select(x => x.ProductName);
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity,
            AllPartialProgresses = partialProgress
        };
    }
}
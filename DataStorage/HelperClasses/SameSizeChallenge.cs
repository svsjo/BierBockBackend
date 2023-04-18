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

        if (!string.IsNullOrEmpty(searchString))
        {
            done = drunkBeers.Where(x => unitNormalizer.NormalizeQuantity(x.Quantity) == searchString)?.Count() ?? 0;
        }
        else /* Wenn nicht gesetzt, kann es jede beliebige Größe sein */
        {
            var sizeGroups = drunkBeers.GroupBy(x => unitNormalizer.NormalizeQuantity(x.Quantity));
            done = sizeGroups.Select(x => x.Count()).Max();
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity
        };
    }
}
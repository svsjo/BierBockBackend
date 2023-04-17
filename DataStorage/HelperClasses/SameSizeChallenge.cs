using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace DataStorage.HelperClasses;

public class SameSizeChallenge : Challenge
{
    public string Size { get; set; } = string.Empty;

    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        var unitNormalizer = new UnitNormalizer();

        int done;

        var drunkBeers = drinkActions.Select(x => x.Product);

        if (!string.IsNullOrEmpty(Size))
        {
            done = drunkBeers.Where(x => unitNormalizer.NormalizeQuantity(x.Quantity) == Size)?.Count() ?? 0;
        }
        else /* Wenn nicht gesetzt, kann es jede beliebige Größe sein */
        {
            var sizeGroups = drunkBeers.GroupBy(x => unitNormalizer.NormalizeQuantity(x.Quantity));
            done = sizeGroups.Select(x => x.Count()).Max();
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = this.NeededQuantity
        };
    }
}
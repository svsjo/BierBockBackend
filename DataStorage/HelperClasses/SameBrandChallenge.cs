
namespace DataStorage.HelperClasses;

public class SameBrandChallenge : Challenge
{
    public string BrandName { get; set; } = string.Empty;

    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        int done;

        var drunkBeers = drinkActions.Select(x => x.Product);

        if (!string.IsNullOrEmpty(BrandName))
        {
            done = drunkBeers.Where(x => x.Brands == BrandName)?.Count() ?? 0;
        }
        else /* Wenn nicht gesetzt, kann es jede beliebige Marke sein */
        {
            var brandGroups = drunkBeers.GroupBy(x => x.Brands);
            done = brandGroups.Select(x => x.Count()).Max();
        }

        return new ChallengeProgress
        {
            Done = done,
            Total = this.NeededQuantity
        };
    }
}
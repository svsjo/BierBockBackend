using DataStorage;

namespace DataStorage.HelperClasses;

public class DiffrentBrandChallenge : Challenge
{
    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        var drunkBeers = drinkActions.Select(x => x.Product);

        var brandGroups = drunkBeers.GroupBy(x => x.Brands);
        var done = brandGroups.Count();

        return new ChallengeProgress
        {
            Done = done,
            Total = this.NeededQuantity
        };
    }
}
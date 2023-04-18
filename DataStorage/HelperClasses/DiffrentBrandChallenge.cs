using DataStorage;

namespace DataStorage.HelperClasses;

public class DiffrentBrandChallenge : IChallengeValidator
{
    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString, int neededQuantity)
    {
        var drunkBeers = drinkActions.Select(x => x.Product);

        var brandGroups = drunkBeers.GroupBy(x => x.Brands);
        var done = brandGroups.Count();

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity
        };
    }
}
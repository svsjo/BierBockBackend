using DataStorage;

namespace DataStorage.HelperClasses;

public class DiffrentBeerChallenge : Challenge
{
    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        var drunkBeers = drinkActions.Select(x => x.Product);

        var beerGroups = drunkBeers.GroupBy(x => x.Code);
        var done = beerGroups.Count();

        return new ChallengeProgress
        {
            Done = done,
            Total = this.NeededQuantity
        };
    }
}
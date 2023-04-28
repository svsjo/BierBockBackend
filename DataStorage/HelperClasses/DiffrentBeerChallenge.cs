namespace DataStorage.HelperClasses;

public class DiffrentBeerChallenge : IChallengeValidator
{
    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString,
        int neededQuantity)
    {
        var drunkBeers = drinkActions.Select(x => x.Product);

        var beerGroups = drunkBeers.GroupBy(x => x.Code).ToList();
        var done = beerGroups.Count();

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity,
            AllPartialProgresses = beerGroups.Select(x => x.First().ProductName)
        };
    }
}
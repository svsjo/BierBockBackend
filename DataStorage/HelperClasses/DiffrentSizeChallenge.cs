namespace DataStorage.HelperClasses;

public class DiffrentSizeChallenge : IChallengeValidator
{
    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString,
        int neededQuantity)
    {
        var drunkBeers = drinkActions.Select(x => x.Product);

        var sizeGroups = drunkBeers.GroupBy(x => UnitNormalizer.NormalizeQuantity(x.Quantity)).ToList();
        var done = sizeGroups.Count();

        return new ChallengeProgress
        {
            Done = done,
            Total = neededQuantity,
            AllPartialProgresses = sizeGroups.Select(x => x.First().Quantity)
        };
    }
}
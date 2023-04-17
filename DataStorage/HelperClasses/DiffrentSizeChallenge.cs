namespace DataStorage.HelperClasses;

public class DiffrentSizeChallenge : Challenge
{
    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        var unitNormalizer = new UnitNormalizer();

        var drunkBeers = drinkActions.Select(x => x.Product);

        var sizeGroups = drunkBeers.GroupBy(x => unitNormalizer.NormalizeQuantity(x.Quantity));
        var done = sizeGroups.Count();

        return new ChallengeProgress
        {
            Done = done,
            Total = this.NeededQuantity
        };
    }
}
namespace DataStorage.HelperClasses;

public interface IChallengeValidator
{
    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString,
        int neededQuantity);
}
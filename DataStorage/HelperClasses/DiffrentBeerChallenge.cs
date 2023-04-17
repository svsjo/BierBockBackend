using DataStorage;

namespace DataStorage.HelperClasses;

public class DiffrentBeerChallenge : Challenge
{
    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        throw new NotImplementedException();
    }
}
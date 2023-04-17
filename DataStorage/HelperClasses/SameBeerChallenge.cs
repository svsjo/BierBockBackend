using BierBockBackend.Data;

namespace DataStorage.HelperClasses;

public class SameBeerChallenge : Challenge
{
    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        throw new NotImplementedException();
    }
}
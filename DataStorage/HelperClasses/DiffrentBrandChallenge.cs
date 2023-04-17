using BierBockBackend.Data;

namespace DataStorage.HelperClasses;

public class DiffrentBrandChallenge : Challenge
{
    public override ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions)
    {
        throw new NotImplementedException();
    }
}
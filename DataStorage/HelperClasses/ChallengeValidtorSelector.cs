namespace DataStorage.HelperClasses;

public class ChallengeValidtorSelector
{
    private readonly DiffrentBeerChallenge _diffrentBeerChallenge = new();
    private readonly DiffrentBrandChallenge _diffrentBrandChallenge = new();
    private readonly DiffrentSizeChallenge _diffrentSizeChallenge = new();
    private readonly SameBeerChallenge _sameBeerChallenge = new();
    private readonly SameBrandChallenge _sameBrandChallenge = new();
    private readonly SameSizeChallenge _sameSizeChallenge = new();

    public ChallengeProgress ValidateChallengeProgress(ICollection<DrinkAction> drinkActions, string searchString,
        int neededQuantity, ChallengeType challengeType)
    {
        return challengeType switch
        {
            ChallengeType.DifferentBeer => _diffrentBeerChallenge.ValidateChallengeProgress(drinkActions, searchString,
                neededQuantity),
            ChallengeType.SameBeer => _sameBeerChallenge.ValidateChallengeProgress(drinkActions, searchString,
                neededQuantity),
            ChallengeType.DifferentBrand => _diffrentBrandChallenge.ValidateChallengeProgress(drinkActions,
                searchString, neededQuantity),
            ChallengeType.SameBrand => _sameBrandChallenge.ValidateChallengeProgress(drinkActions, searchString,
                neededQuantity),
            ChallengeType.DifferentSize => _diffrentSizeChallenge.ValidateChallengeProgress(drinkActions, searchString,
                neededQuantity),
            ChallengeType.SameSize => _sameSizeChallenge.ValidateChallengeProgress(drinkActions, searchString,
                neededQuantity),
            _ => throw new NotImplementedException()
        };
    }
}
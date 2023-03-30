using BierBockBackend.Data;
using DataStorage;
using DataStorage.HelpRelations;
using Microsoft.AspNetCore.Mvc;

namespace BierBockBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class BierBockController : ControllerBase
{
    private readonly AppDatabaseContext _dbAppDatabaseContext;

    public BierBockController(AppDatabaseContext dbAppDatabaseContext)
    {
        _dbAppDatabaseContext = dbAppDatabaseContext;
    }

    [HttpGet("allDrinkActions", Name = "GetAllDrinkActions")]
    public IEnumerable<DrinkAction> GetAllDrinkActions()
    {
        return _dbAppDatabaseContext.GetDrinkActions();
    }

    [HttpGet("allProducts", Name = "GetAllProducts")]
    public IEnumerable<Product> GetAllProducts()
    {
        return _dbAppDatabaseContext.GetProducts();
    }

    [HttpPost("exampleData", Name = "AddExampleData")]
    public void AddExampleData()
    {
        #region Allgemein

        var user = new User
        {
            Token = "123456",
            Name = "Max Mustermann",
            PasswordHash = "Password123",
            Email = "max.mustermann@example.com",
            FavouriteBeerCode = "3080216052885",
            BirthDate = new DateOnly(1990, 1, 1),
            Points = 10,
            Latitude = 48.1351,
            Longitude = 11.5820,
            Altitude = 100
        };

        var user1 = new User
        {
            Token = "abcdef",
            Name = "Anna Müller",
            PasswordHash = "Password123",
            Email = "anna.mueller@example.com",
            FavouriteBeerCode = "WEISS",
            BirthDate = new DateOnly(1995, 6, 15),
            Points = 5,
            Latitude = 51.5074,
            Longitude = -0.1278,
            Altitude = 10,
        };

        var user2 = new User
        {
            Token = "ghijkl",
            Name = "Hans Schmidt",
            PasswordHash = "Password456",
            Email = "hans.schmidt@example.com",
            FavouriteBeerCode = "3080216049632",
            BirthDate = new DateOnly(1985, 3, 2),
            Points = 15,
            Latitude = 40.7128,
            Longitude = -74.0060,
            Altitude = 50,
        };


        var drinkAction = new DrinkAction
        {
            BeerCode = "3080216052885",
            Time = DateTime.Now,
            Latitude = 48.1351,
            Longitude = 11.5820,
            Altitude = 100,
            UserId = user.Id,
            User = user,
        };

        var drinkAction1 = new DrinkAction
        {
            BeerCode = "3080216049632",
            Time = DateTime.Now.AddDays(-2),
            Latitude = 51.5074,
            Longitude = -0.1278,
            Altitude = 10,
            UserId = user1.Id,
            User = user1,
        };

        var drinkAction2 = new DrinkAction
        {
            BeerCode = "3080216049632",
            Time = DateTime.Now.AddDays(-1),
            Latitude = 40.7128,
            Longitude = -74.0060,
            Altitude = 50,
            UserId = user2.Id,
            User = user2,
        };


        var challengePart = new ChallengePart
        {
            Description = "Trink ein Bier",
            BeerCode = "3080216052885",
            Quantity = 1,
        };

        var challengePart1 = new ChallengePart
        {
            Description = "Trink ein Weißbier",
            BeerCode = "3119783007483",
            Quantity = 1,
        };

        var challengePart2 = new ChallengePart
        {
            Description = "Trink ein Pils",
            BeerCode = "3119783007483",
            Quantity = 1,
        };


        var challenge = new Challenge
        {
            PossiblePoints = 50,
            Description = "Trink jeden Tag ein Bier",
            StartDate = DateTime.Now.AddDays(-7),
            EndDate = DateTime.Now.AddDays(7)
        };

        var challenge1 = new Challenge
        {
            PossiblePoints = 100,
            Description = "Trink 10 Biere in einer Woche",
            StartDate = DateTime.Now.AddDays(-7),
            EndDate = DateTime.Now.AddDays(7),
        };

        var challenge2 = new Challenge
        {
            PossiblePoints = 75,
            Description = "Trink 5 verschiedene Biere in einer Woche",
            StartDate = DateTime.Now.AddDays(-14),
            EndDate = DateTime.Now.AddDays(14),
        };

        #endregion

        #region Zuordnung Challenge <> ChallengePart

        var challengePartAssignment1 = new ChallengePartChallenge()
        {
            Challenge = challenge1,
            ChallengeId = challenge1.Id,
            ChallengePart = challengePart,
            ChallengePartId = challengePart.Id
        };

        var challengePartAssignment2 = new ChallengePartChallenge()
        {
            Challenge = challenge1,
            ChallengeId = challenge1.Id,
            ChallengePart = challengePart1,
            ChallengePartId = challengePart1.Id
        };

        challenge1.PartialChallenges.Add(challengePartAssignment1);
        challenge1.PartialChallenges.Add(challengePartAssignment2);
        challengePart.Challenges.Add(challengePartAssignment1);
        challengePart1.Challenges.Add(challengePartAssignment2);

        #endregion

        #region Zuordnung Challenge <> User

        var challengeUser = new ChallengeUser
        {
            UserId = user.Id,
            ChallengeId = challenge.Id,
            User = user,
            Challenge = challenge
        };

        var challenge1User = new ChallengeUser()
        {
            UserId = user.Id,
            ChallengeId = challenge1.Id,
            User = user,
            Challenge = challenge1
        };

        var challengeUser1 = new ChallengeUser()
        {
            UserId = user1.Id,
            ChallengeId = challenge.Id,
            User = user1,
            Challenge = challenge
        };

        user.UserChallenges.Add(challengeUser);
        user.UserChallenges.Add(challenge1User);
        user1.UserChallenges.Add(challengeUser1);
        challenge.Users.Add(challengeUser);
        challenge.Users.Add(challengeUser1);
        challenge1.Users.Add(challenge1User);

        #endregion

        #region Zuordnung DrinkingActions <> User

        user.AllDrinkingActions.Add(drinkAction);
        user1.AllDrinkingActions.Add(drinkAction1);
        user2.AllDrinkingActions.Add(drinkAction2);

        #endregion

        #region In Datenbank schreiben

        _dbAppDatabaseContext.AddChallengePart(challengePart);
        _dbAppDatabaseContext.AddChallengePart(challengePart1);
        _dbAppDatabaseContext.AddChallengePart(challengePart2);
        _dbAppDatabaseContext.AddChallenge(challenge);
        _dbAppDatabaseContext.AddChallenge(challenge1);
        _dbAppDatabaseContext.AddChallenge(challenge2);
        _dbAppDatabaseContext.AddUser(user);
        _dbAppDatabaseContext.AddUser(user1);
        _dbAppDatabaseContext.AddUser(user2);
        _dbAppDatabaseContext.AddDrinkAction(drinkAction);
        _dbAppDatabaseContext.AddDrinkAction(drinkAction1);
        _dbAppDatabaseContext.AddDrinkAction(drinkAction2);

        #endregion
    }


    [HttpPost("addProducts", Name = "AddAllProducts")]
    public void AddAllProducts()
    {
        var openFoodFacts = new OpenFoodFactsApi();
        var allBeers = openFoodFacts.GetBeerData().Result;
        _dbAppDatabaseContext.AddProducts(allBeers);
    }
}
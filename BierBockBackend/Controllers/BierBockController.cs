using System.IdentityModel.Tokens.Jwt;
using BierBockBackend.Data;
using DataStorage;
using DataStorage.HelpRelations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security;
using Microsoft.AspNetCore.Http.Metadata;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BierBockBackend.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BierBockController : ControllerBase
{
    private readonly AppDatabaseContext _dbAppDatabaseContext;

    public BierBockController(AppDatabaseContext dbAppDatabaseContext)
    {
        _dbAppDatabaseContext = dbAppDatabaseContext;
    }

    #region Echte Schnittstelle

    [HttpGet("ownUserData", Name = "GetOwnUserData")]
    public RequestStatus<object> GetOwnUserData()
    {

        var user = GetCurrentUser();
        var result = user != default
            ? new
            {
                Name = user.Name,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Location = user.Location
            }
            : default;

        var sucess = result != default;
        var status = sucess ? Status.Successful : Status.NoResults;

        return new RequestStatus<object>
        {
            Status = status,
            Result = result
        };
    }

    [HttpGet("ownChallenges", Name = "GetOwnChallenges")]
    public RequestStatus<IEnumerable<Challenge>> GetOwnChallenges()
    {
        var user = GetCurrentUser();

        var results = user.UserChallenges.Select(x => x.Challenge);

        var status = (results!.Any() ? Status.Successful : Status.NoResults);

        return new RequestStatus<IEnumerable<Challenge>>
        {
            Status = status,
            Result = results
        };
    }

    [HttpGet("allDrinkActions", Name = "GetAllDrinkActions")]
    public RequestStatus<IEnumerable<DrinkAction>> GetAllDrinkActions()
    {
        var results = _dbAppDatabaseContext.GetDrinkActions();
    
        return new RequestStatus<IEnumerable<DrinkAction>>
        {
            Result = results
        };
    }

    [HttpGet("bestSearchResults", Name = "GetBestSearchResults")]
    public RequestStatus<IEnumerable<Product>> GetBestSearchResults(string searchString)
    {

        var results = _dbAppDatabaseContext.GetProducts()
            .Where(x => x.ProductName.ToLower().Contains(searchString.ToLower()) ||
                        (x.Brands != default && x.Brands.ToLower().Contains(searchString.ToLower())))
            .Take(25);

        return new RequestStatus<IEnumerable<Product>>
        {
            Result = results
        };
    }

    [HttpGet("ownScore", Name = "GetOwnScore")]
    public RequestStatus<int> GetOwnScore()
    {
        var user = GetCurrentUser();
        var sucess = user != default;
        var status = sucess ? Status.Successful : Status.NoResults;
        var result = sucess ? user!.Points : 0;

        return new RequestStatus<int>
        {
            Status = status,
            Result = result
        };
    }

    [HttpGet("topRankedUsers", Name = "GetTopRankedUsers")]
    public RequestStatus<IEnumerable<object>> GetTopRankedUsers()
    {
        var user = GetCurrentUser();
        var users = user != default
            ? _dbAppDatabaseContext.GetUsers()
                .OrderBy(x => x.Points)
                .Take(15)
                .ToList()
            : default;

        var results = users?.Select((value, index) => new
        {
            Rank = index + 1,
            Name = value.Name,
            Points = value.Points
        });

        var status = results!.Any() ? Status.Successful : Status.NoResults;

        return new RequestStatus<IEnumerable<object>>
        {
            Status = status,
            Result = results
        };
    }

    [HttpGet("ownRanking", Name = "GetOwnRanking")]
    public RequestStatus<int> GetOwnRanking()
    {
        var user = GetCurrentUser();
        var rank = user != default
            ? _dbAppDatabaseContext.GetUsers()
                .OrderBy(x => x.Points)
                .ToList()
                .IndexOf(user)
            : default;

        var status = Status.Successful;

        return new RequestStatus<int>
        {
            Status = status,
            Result = rank
        };
    }

    [HttpGet("barcodeData", Name = "GetBarcodeData")]
    public RequestStatus<Product> GetBarcodeData(string barcode)
    {
        var user = GetCurrentUser();

        var product = user != default
            ? _dbAppDatabaseContext.GetProducts()
                .FirstOrDefault(x => x.Code == barcode)
            : default;

        var status = (product != default ? Status.Successful : Status.NoResults);

        return new RequestStatus<Product>
        {
            Status = status,
            Result = product
        };
    }

    [HttpGet("ownDrinkProgress", Name = "GetOwnDrinkProgress")]
    public RequestStatus<IEnumerable<DrinkAction>> GetOwnDrinkProgress( DateTime toTime = default, DateTime fromTime = default)
    {
        var user = GetCurrentUser();

        var result = user?.AllDrinkingActions.ToList();

        if (toTime != default) result = result?.Where(x => x.Time < toTime).ToList();
        if (fromTime != default) result = result?.Where(x => x.Time > fromTime).ToList();

        var status = (result != default ? Status.Successful : Status.NoResults);

        return new RequestStatus<IEnumerable<DrinkAction>>
        {
            Status = status,
            Result = result
        };
    }

    [HttpPost("newDrinkAction", Name = "SetNewDrinkAction")]
    public RequestStatus<object> SetNewDrinkAction( Coordinate coordinate, string beerCode)
    {
        var user = GetCurrentUser();

        var product = user != default
            ? _dbAppDatabaseContext.GetProducts()
                .FirstOrDefault(x => x.Code == beerCode)
            : default;

        var drinkAction = product != default
            ? new DrinkAction
            {
                Location = coordinate,
                User = user!,
                Product = product,
            }
            : default;

        if (drinkAction != default)
        {
            user!.AllDrinkingActions.Add(drinkAction);
            _dbAppDatabaseContext.AddDrinkAction(drinkAction);
        }

        var status = (product != default ? Status.Successful : Status.NoResults);

        return new RequestStatus<object>
        {
            Status = status,
        };
    }

    [HttpPost("actualisateUserPosition", Name = "ActualisateUserPosition")]
    public RequestStatus<object> ActualisateUserPosition(Coordinate coordinate)
    {
        var user = GetCurrentUser();
        var status =  Status.Successful;

        if (user != default)
        {
            user.Location = coordinate;
        }


        return new RequestStatus<object>
        {
            Status = status,
        };
    }

    [HttpPost("actualisateUserBasicData", Name = "ActualisateUserBasicData")]
    public RequestStatus<object> ActualisateUserBasicData(
        string? newName = default, string? newBirthDate = default, string? newFavouriteBeerCode = default)
        {
            var user = GetCurrentUser();
            var status =  Status.Successful;

        if (user != default)
        {
            if (newName != default) user.Name = newName;
            if (newBirthDate != default) user.BirthDate = newBirthDate;
            if (newFavouriteBeerCode != default)
            {
                var beer = _dbAppDatabaseContext.GetProducts().FirstOrDefault(x => x.Code == newFavouriteBeerCode);
                if (beer == default) status = Status.NoResults;
                else
                {
                    user.FavouriteBeer = beer;
                }
            }
        }

        return new RequestStatus<object>
        {
            Status = status,
        };
    }

    #endregion

    #region Testweise Schnittstelle

    /*
    [HttpGet("Temp_allProducts", Name = "GetAllProducts")]
    public IEnumerable<Product> GetAllProducts()
    {
        return _dbAppDatabaseContext.GetProducts();
    }

    [HttpPost("Temp_exampleData", Name = "AddExampleData")]
    public void AddExampleData()
    {
        #region Allgemein

        var beer1 = new Product
        {
            Code = "1234",
            ProductName = "Apple",
            Brands = "BrandA",
            ImageUrl = "https://example.com/apple.png",
            Categories = "Fruits",
            Quantity = "1 kg",
            GenericName = "Fruit",
        };

        var beer2 = new Product
        {
            Code = "5678",
            ProductName = "Banana",
            Brands = "BrandB",
            ImageUrl = "https://example.com/banana.png",
            Categories = "Fruits",
            Quantity = "500 g",
            GenericName = "Fruit",
        };


        var user = new User
        {
            Token = "123456",
            Name = "Max Mustermann",
            PasswordHash = "Password123",
            Email = "max.mustermann@example.com",
            FavouriteBeer = beer1,
            BeerId = beer1.Id,
            BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
            Points = 10,
            Location = new Coordinate()
            {
                Latitude = 48.1351,
                Longitude = 11.5820,
                Altitude = 100
            }
        };

        var user1 = new User
        {
            Token = "abcdef",
            Name = "Anna Müller",
            PasswordHash = "Password123",
            Email = "anna.mueller@example.com",
            FavouriteBeer = beer1,
            BeerId = beer1.Id,
            BirthDate = new DateOnly(1995, 6, 15).ToLongDateString(),
            Points = 5,
            Location = new Coordinate()
            {
                Latitude = 51.5074,
                Longitude = -0.1278,
                Altitude = 10,
            }
        };

        var user2 = new User
        {
            Token = "ghijkl",
            Name = "Hans Schmidt",
            PasswordHash = "Password456",
            Email = "hans.schmidt@example.com",
            FavouriteBeer = beer2,
            BeerId = beer2.Id,
            BirthDate = new DateOnly(1985, 3, 2).ToLongDateString(),
            Points = 15,
            Location = new Coordinate()
            {
                Latitude = 40.7128,
                Longitude = -74.0060,
                Altitude = 50,
            }
        };


        var drinkAction = new DrinkAction
        {
            Product = beer1,
            //ProductId = beer1.Id,
            Time = DateTime.Now,
            Location = new Coordinate()
            {
                Latitude = 48.1351,
                Longitude = 11.5820,
                Altitude = 100,
            },
            //UserId = user.Id,
            User = user,
        };

        var drinkAction1 = new DrinkAction
        {
            Product = beer1,
            //ProductId = beer1.Id,
            Time = DateTime.Now.AddDays(-2),
            Location = new Coordinate()
            {
                Latitude = 51.5074,
                Longitude = -0.1278,
                Altitude = 10,
            },
            //UserId = user1.Id,
            User = user1,
        };

        var drinkAction2 = new DrinkAction
        {
            Product = beer2,
            //ProductId = beer2.Id,
            Time = DateTime.Now.AddDays(-1),
            Location = new Coordinate()
            {
                Latitude = 40.7128,
                Longitude = -74.0060,
                Altitude = 50,
            },
            //UserId = user2.Id,
            User = user2,
        };


        var challengePart = new ChallengePart
        {
            Description = "Trink ein Bier",
            Beer = beer1,
            BeerId = beer1.Id,
            Quantity = 1,
        };

        var challengePart1 = new ChallengePart
        {
            Description = "Trink ein Weißbier",
            Beer = beer1,
            BeerId = beer1.Id,
            Quantity = 1,
        };

        var challengePart2 = new ChallengePart
        {
            Description = "Trink ein Pils",
            Beer = beer2,
            BeerId = beer2.Id,
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


    [HttpPost("Temp_addProducts", Name = "AddAllProducts")]
    public void AddAllProducts()
    {
        var openFoodFacts = new OpenFoodFactsApi();
        var allBeers = openFoodFacts.GetBeerData().Result;
        _dbAppDatabaseContext.AddProducts(allBeers);
    }
    */

    #endregion

    User GetCurrentUser()
    {
        var name = HttpContext.User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return _dbAppDatabaseContext.GetUsers()
            .FirstOrDefault(x => x.Name == name);
    }
}
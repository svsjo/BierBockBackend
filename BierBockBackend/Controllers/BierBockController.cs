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
                user.UserName,
                user.Name,
                user.VorName,
                user.BirthDate,
                user.Email,
                user.Location,
                user.Wohnort,
                user.FavouriteBeer.ProductName
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

        var results = user.UserChallenges
            .Select(x => x.Challenge)
            .ToList();

        // TODO: Challenge Fortschritt berechnen

        var status = (results.Any() ? Status.Successful : Status.NoResults);

        return new RequestStatus<IEnumerable<Challenge>>
        {
            Status = status,
            Result = results
        };
    }

    [HttpGet("allDrinkActions", Name = "GetAllDrinkActions")]
    public RequestStatus<IEnumerable<object>> GetAllDrinkActions(string? searchString = default, DateTime fromTime = default, DateTime toTime = default)
    {
        var results = _dbAppDatabaseContext.GetDrinkActions();

        if (fromTime != default) results = results.Where(x => x.Time > fromTime);
        if (toTime != default) results = results.Where(x => x.Time < toTime);
        if (searchString != default)
            results = results
                .Where(x => (x.Product.Brands != null && x.Product.Brands.ToLower().Contains(searchString.ToLower())) ||
                            x.Product.ProductName.ToLower().Contains(searchString.ToLower()) ||
                            (x.Product.Categories != null && x.Product.Categories.ToLower().Contains(searchString.ToLower())))
                .OrderByDescending(x => x.Time);


        var status = results.Any() ? Status.Successful : Status.NoResults;

        return new RequestStatus<IEnumerable<object>>
        {
            Result = results.Select(x => new
            {
                x.Location,
                x.Time
            }),
            Status = status
        };
    }

    [HttpGet("bestSearchResults", Name = "GetBestSearchResults")]
    public RequestStatus<IEnumerable<object>> GetBestSearchResults(string searchString)
    {
        var results = _dbAppDatabaseContext.GetProducts()
            .Where(x => x.ProductName.ToLower().Contains(searchString.ToLower()) ||
                        (x.Brands != default && x.Brands.ToLower().Contains(searchString.ToLower())))
            .Take(25);

        var status = results.Any() ? Status.Successful : Status.NoResults;

        return new RequestStatus<IEnumerable<object>>
        {
            Result = results,
            Status = status
        };
    }

    [HttpGet("ownScore", Name = "GetOwnScore")]
    public RequestStatus<int> GetOwnScore()
    {
        var user = GetCurrentUser();
        var status = Status.Successful;
        var result = user.Points;

        return new RequestStatus<int>
        {
            Status = status,
            Result = result
        };
    }

    [HttpGet("topRankedUsers", Name = "GetTopRankedUsers")]
    public RequestStatus<object> GetTopRankedUsers()
    {
        var user = GetCurrentUser();

        var users = _dbAppDatabaseContext.GetUsers()
            .OrderByDescending(x => x.Points);

        var results = users.Select((value, index) => new
        {
            Rank = index + 1,
            value.UserName,
            value.Points
        }).ToList();

        var ownRanking = results
            .First(x => x.UserName == user.UserName);

        var status = results.Any() ? Status.Successful : Status.NoResults;

        return new RequestStatus<object>
        {
            Status = status,
            Result = new
            {
                Top25 = results.Take(25),
                Own = ownRanking
            }
        };
    }

    [HttpGet("ownRanking", Name = "GetOwnRanking")]
    public RequestStatus<int> GetOwnRanking()
    {
        var user = GetCurrentUser();

        var rank = _dbAppDatabaseContext.GetUsers()
            .OrderBy(x => x.Points)
            .ToList()
            .IndexOf(user) + 1;

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

        var product = _dbAppDatabaseContext.GetProducts()
            .FirstOrDefault(x => x.Code == barcode);

        var status = (product != default ? Status.Successful : Status.NoResults);

        return new RequestStatus<Product>
        {
            Status = status,
            Result = product
        };
    }

    [HttpGet("ownDrinkProgress", Name = "GetOwnDrinkProgress")]
    public RequestStatus<IEnumerable<object>> GetOwnDrinkProgress(DateTime toTime = default, DateTime fromTime = default)
    {
        var user = GetCurrentUser();

        var results = user?.AllDrinkingActions
            .Select(x => new
            {
                x.Location,
                x.Time,
                x.Product.Code,
                x.Product.ProductName,
                x.Product.Brands
            })
            .OrderByDescending(x => x.Time)
            .ToList();

        if (toTime != default) results = results?.Where(x => x.Time < toTime).ToList();
        if (fromTime != default) results = results?.Where(x => x.Time > fromTime).ToList();

        var status = results != default && results.Any() ? Status.Successful : Status.NoResults;

        return new RequestStatus<IEnumerable<object>>
        {
            Status = status,
            Result = results
        };
    }

    [HttpPost("newDrinkAction", Name = "SetNewDrinkAction")]
    public RequestStatus<object> SetNewDrinkAction(Coordinate coordinate, string beerCode)
    {
        var user = GetCurrentUser();

        var product = _dbAppDatabaseContext.GetProducts()
            .FirstOrDefault(x => x.Code == beerCode);

        var drinkAction = product != default
            ? new DrinkAction
            {
                Location = coordinate,
                User = user,
                Product = product,
            }
            : default;

        if (drinkAction != default)
        {
            user.AllDrinkingActions.Add(drinkAction);
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
        var status = Status.Successful;

        user.Location = coordinate;

        return new RequestStatus<object>
        {
            Status = status,
        };
    }

    [HttpPost("actualisateUserBasicData", Name = "ActualisateUserBasicData")]
    public RequestStatus<object> ActualisateUserBasicData(string? newName = default, string? newVorname = default, string? newWohnort = default, 
        string? newBirthDate = default, string? newFavouriteBeerCode = default, string? newMail = default)
    {
        var user = GetCurrentUser();
        var status = Status.Successful;

        if (newName != default) user.Name = newName;
        if (newVorname != default) user.VorName = newVorname;
        if (newWohnort != default) user.Wohnort = newWohnort;
        if (newBirthDate != default) user.BirthDate = newBirthDate;
        if (newMail != default) user.Email = newMail;
        if (newFavouriteBeerCode != default)
        {
            var beer = _dbAppDatabaseContext.GetProducts().FirstOrDefault(x => x.Code == newFavouriteBeerCode);
            if (beer == default) status = Status.NoResults;
            else user.FavouriteBeer = beer;
        }

        return new RequestStatus<object>
        {
            Status = status,
        };
    }

    #endregion

    private User GetCurrentUser()
    {
        var name = HttpContext.User.Claims
            .FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?
            .Value;

        return _dbAppDatabaseContext.GetUsers()
            .FirstOrDefault(x => x.Name == name)!;
        // TODO: UserName?
    }
}
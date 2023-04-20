using BierBockBackend.Auth;
using BierBockBackend.Data;
using DataStorage;
using DataStorage.HelperClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BierBockBackend.BackgroundServices;

public class DatabaseUpdateService : BackgroundService
{
    private readonly ILogger<DatabaseUpdateService>? _logger;
    private readonly AppDatabaseContext _dbContext;
    private readonly OpenFoodFactsApi _foodFactsApi;
    private readonly IServiceScope scope;

    public DatabaseUpdateService(IServiceScopeFactory serviceScopeFactory)
    {
        scope = serviceScopeFactory.CreateScope();
        this._dbContext = scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();
        _logger = scope.ServiceProvider.GetService<ILogger<DatabaseUpdateService>>();
        _foodFactsApi = new OpenFoodFactsApi();
    }

    public override void Dispose()
    {
        this.scope.Dispose();
        base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Scheduled task started at: {time}", DateTimeOffset.Now);

            await this.InsertNewProducts();
            this.InitBasicUserData();

            _logger.LogInformation("Scheduled task ended at: {time}", DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromDays(3), stoppingToken);
        }
    }

    private async Task InsertNewProducts()
    {
        var products = await _foodFactsApi.GetBeerData();

        foreach (var product in products
                     .Where(product => _dbContext.GetProducts()
                         .All(x => x.Code != product.Code)))
        {
            _dbContext.AddProduct(product);
        }
    }

    private void InitBasicUserData()
    {
        /* Nur bei leerer DB */

        const int users = 30;
        const int drinkActions = 8;
        const int challenges = 6;

        if (_dbContext.GetUsers().Count() < users)
        {
            InitUsers(users);
        }

        if (_dbContext.GetDrinkActions().Count() < drinkActions)
        {
            InitDrinkActions(drinkActions);
        }

        if (_dbContext.GetUsers().First().UserChallenges.Count() < challenges)
        {
            InitChallenges();
        }
    }

    private void InitDrinkActions(int numberDrinkActions)
    {
        var products = _dbContext.GetProducts().Take(numberDrinkActions).ToList();

        var random = new Random();
        var users = _dbContext.GetUsers().ToList();

        foreach (var product in products)
        {
            foreach (var user in users)
            {
                var drinkAction = new DrinkAction
                {
                    Product = product,
                    Time = DateTime.Now,
                    Location = new Coordinate()
                    {
                        Latitude = 48.1351 + random.Next(0, 400),
                        Longitude = 11.5820 + random.Next(0, 400),
                        Altitude = 100 + random.Next(0, 200),
                    },
                    User = user,
                };

                _dbContext.AddDrinkAction(drinkAction);
                user.AllDrinkingActions.Add(drinkAction);
                product.UsedInDrinkActions.Add(drinkAction);
            }
        }

        _dbContext.SaveChanges();
    }

    private void InitUsers(int numberUsers)
    {
        var products = _dbContext.GetProducts().Take(numberUsers).ToList();

        var hash = PasswordHashing.HashPassword("Password123");

        #region mustimax

        var user = new User
        {
            Name = "Mustermann",
            VorName = "Max",
            UserName = "mustimax",
            PasswordHash = hash.Hash,
            PasswordSalt = hash.Salt,
            Email = "max.mustermann@example.com",
            FavouriteBeer = products.ElementAt(0),
            BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
            Points = 10,
            EmailConfirmed = true,
            Location = new Coordinate()
            {
                Latitude = 48.1351,
                Longitude = 11.5820,
                Altitude = 100
            }
        };

        _dbContext.AddUser(user);
        products.ElementAt(0).UsersHavingThisAsFavouriteBeer.Add(user);

        #endregion

        var random = new Random();

        foreach (var product in products)
        {
            var vorname = Guid.NewGuid().ToString();
            var nachname = Guid.NewGuid().ToString();
            var username = vorname[..17] + nachname[..17];
            var mail = username + "@example.com";

            var user2 = new User
            {
                Name = vorname,
                VorName = nachname,
                UserName = username,
                PasswordHash = hash.Hash,
                PasswordSalt = hash.Salt,
                Email = mail,
                FavouriteBeer = product,
                BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
                Points = 10 + random.Next(0, 500),
                EmailConfirmed = true,
                Location = new Coordinate()
                {
                    Latitude = 48.1351 + random.Next(0, 400),
                    Longitude = 11.5820 + random.Next(0, 400),
                    Altitude = 100 + random.Next(0, 200)
                }
            };

            _dbContext.AddUser(user2);
            product.UsersHavingThisAsFavouriteBeer.Add(user2);
        }

        _dbContext.SaveChanges();
    }

    private void InitChallenges()
    {
        var challenge = new Challenge()
        {
            ChallengeType = ChallengeType.DifferentBrand,
            Description = "Trinke von drei unterschiedlichen Marken",
            PossiblePoints = 30,
            NeededQuantity = 3
        };

        var challenge2 = new Challenge()
        {
            ChallengeType = ChallengeType.SameBrand,
            Description = "Trinke drei Bier der Marke Alpirsbacher",
            SearchString = "Alpirsbacher",
            PossiblePoints = 50,
            NeededQuantity = 3
        };

        var challenge3 = new Challenge()
        {
            ChallengeType = ChallengeType.DifferentBeer,
            Description = "Trinke fünf unterschiedliche Bier",
            PossiblePoints = 20,
            NeededQuantity = 5
        };

        var challenge4 = new Challenge()
        {
            ChallengeType = ChallengeType.SameBeer,
            Description = "Trinke fünf Alpirsbacher Spezial",
            SearchString = "Alpirsbacher Spezial",
            PossiblePoints = 30,
            NeededQuantity = 5
        };

        var challenge5 = new Challenge()
        {
            ChallengeType = ChallengeType.DifferentSize,
            Description = "Trinke drei Biere unterschiedlicher Größe",
            PossiblePoints = 20,
            NeededQuantity = 3
        };

        var challenge6 = new Challenge()
        {
            ChallengeType = ChallengeType.SameSize,
            Description = "Trinke drei Bier der Größe 0,5L",
            SearchString = "0,5L",
            PossiblePoints = 50,
            NeededQuantity = 3
        };

        _dbContext.AddChallenge(challenge);
        _dbContext.AddChallenge(challenge2);
        _dbContext.AddChallenge(challenge3);
        _dbContext.AddChallenge(challenge4);
        _dbContext.AddChallenge(challenge5);
        _dbContext.AddChallenge(challenge6);

        AddChallengesToUsers();
    }

    private void AddChallengesToUsers()
    {
        var challenges = _dbContext.GetChallenges();
        var users = _dbContext.GetUsers();

        foreach (var user in users)
        {
            foreach (var challenge in challenges)
            {
                var challengeUser = new ChallengeUser()
                {
                    Challenge = challenge,
                    User = user,
                };

                challenge.Users.Add(challengeUser);
                user.UserChallenges.Add(challengeUser);
            }
        }

        _dbContext.SaveChanges();
    }
}
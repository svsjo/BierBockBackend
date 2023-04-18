using BierBockBackend.Data;
using DataStorage;
using DataStorage.HelperClasses;

namespace BierBockBackend.BackgroundServices;

public class DatabaseUpdateService : BackgroundService
{
    private readonly ILogger<DatabaseUpdateService> _logger;
    private readonly AppDatabaseContext _dbContext;
    private readonly OpenFoodFactsApi _foodFactsApi;

    public DatabaseUpdateService(ILogger<DatabaseUpdateService> logger, AppDatabaseContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _foodFactsApi = new OpenFoodFactsApi();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Scheduled task started at: {time}", DateTimeOffset.Now);

            await this.InsertNewProducts();
            this.InitBasicUserData(); // nur testweise - Mock Daten

            _logger.LogInformation("Scheduled task ended at: {time}", DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
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

        if (!_dbContext.GetUsers().Any())
        {
            var user = new User
            {
                Token = "123456",
                Name = "Mustermann",
                VorName = "Max",
                UserName = "mustimax",
                PasswordHash = "Password123",
                Email = "max.mustermann@example.com",
                FavouriteBeer = _dbContext.GetProducts().First(),
                BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
                Points = 10,
                Location = new Coordinate()
                {
                    Latitude = 48.1351,
                    Longitude = 11.5820,
                    Altitude = 100
                }
            };

            _dbContext.AddUser(user);
            _dbContext.GetProducts().First().UsersHavingThisAsFavouriteBeer.Add(user);
            _dbContext.SaveChanges();
        }

        if (!_dbContext.GetUsers().First().AllDrinkingActions.Any())
        {
            var drinkAction = new DrinkAction
            {
                Product = _dbContext.GetProducts().First(),
                Time = DateTime.Now,
                Location = new Coordinate()
                {
                    Latitude = 48.1351,
                    Longitude = 11.5820,
                    Altitude = 100,
                },
                User = _dbContext.GetUsers().First(),
            };

            _dbContext.AddDrinkAction(drinkAction);

            _dbContext.GetUsers().First().AllDrinkingActions.Add(drinkAction);
            _dbContext.SaveChanges();

            _dbContext.GetProducts().First().UsedInDrinkActions.Add(drinkAction);
            _dbContext.SaveChanges();
        }

        if (!_dbContext.GetUsers().First().UserChallenges.Any())
        {
            var challenge = new Challenge()
            {
                ChallengeType = ChallengeType.DifferentBeer,
                Description = "Trinke drei Unterschiedliche Bier",
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

            _dbContext.GetUsers().First().UserChallenges.Add(challenge);
            _dbContext.GetUsers().First().UserChallenges.Add(challenge2);
            _dbContext.SaveChanges();
        }
    }
}
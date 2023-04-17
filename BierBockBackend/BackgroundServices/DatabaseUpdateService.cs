using BierBockBackend.Data;
using DataStorage;
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
        if (!_dbContext.GetUsers().Any()) /* Nur bei leerer DB */
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
            _dbContext.GetProducts().First().UsersHavingThisAsFavouriteBeer.Add(user);
            _dbContext.SaveChanges();
        }
    }
}
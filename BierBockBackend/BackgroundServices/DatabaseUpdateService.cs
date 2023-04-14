using BierBockBackend.Data;
using DataStorage;
using DataStorage.HelpRelations;

namespace BierBockBackend.BackgroundServices;

public class DatabaseUpdateService : BackgroundService
{
    private readonly ILogger<DatabaseUpdateService> _logger;
    private readonly AppDatabaseContext _dbContext;

    public DatabaseUpdateService(ILogger<DatabaseUpdateService> logger, AppDatabaseContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Scheduled task started at: {time}", DateTimeOffset.Now);

            this.Insert();
            this.InitBasicUserData();

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private void Insert()
    {
        if (_dbContext.GetProducts().Any()) return;

        var productsTask = new OpenFoodFactsApi().GetBeerData();
        var products = productsTask.Result;
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

            var challenge = new Challenge
            {
                PossiblePoints = 50,
                Description = "Trink jeden Tag ein Bier",
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now.AddDays(7)
            };

            _dbContext.AddChallenge(challenge);

            var challengeUser = new ChallengeUser
            {
                User = _dbContext.GetUsers().First(),
                Challenge = _dbContext.GetChallenge().First()
            };

            _dbContext.GetUsers().First().UserChallenges.Add(challengeUser);
            _dbContext.GetChallenge().First().Users.Add(challengeUser);
            _dbContext.SaveChanges();

            _dbContext.GetProducts().First().UsedInDrinkActions.Add(drinkAction);
            _dbContext.GetProducts().First().UsersHavingThisAsFavouriteBeer.Add(user);
            _dbContext.SaveChanges();
        }
    }
}
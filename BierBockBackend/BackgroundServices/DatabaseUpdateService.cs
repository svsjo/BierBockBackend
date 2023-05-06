#region

using BierBockBackend.Data;
using DataStorage;

#endregion

namespace BierBockBackend.BackgroundServices;

public class DatabaseUpdateService : BackgroundService
{
    private readonly AppDatabaseContext _dbContext;
    private readonly OpenFoodFactsClient _foodFactsClient;
    private readonly ILogger<DatabaseUpdateService>? _logger;
    private readonly TestDataHolder _testDataHolder;
    private readonly IServiceScope _scope;

    public DatabaseUpdateService(IServiceScopeFactory serviceScopeFactory)
    {
        _scope = serviceScopeFactory.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();
        _logger = _scope.ServiceProvider.GetService<ILogger<DatabaseUpdateService>>();
        _foodFactsClient = new OpenFoodFactsClient();
        _testDataHolder = new TestDataHolder(_dbContext);
    }

    public override void Dispose()
    {
        _scope.Dispose();
        base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger?.LogInformation("Scheduled task started at: {time}", DateTimeOffset.Now);

            await InsertNewProducts();
            InitBasicUserData();

            _logger?.LogInformation("Scheduled task ended at: {time}", DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromDays(3), stoppingToken);
        }
    }

    private async Task InsertNewProducts()
    {
        /* Nur fehlende Produkte */

        var products = await _foodFactsClient.GetBeerData();
        var newProducts = 0;

        foreach (var product in products
                     .Where(product => _dbContext.GetProducts()
                         .All(x => x.Code != product.Code)))
        {
            _dbContext.AddProduct(product);
            newProducts++;
        }

        _logger?.LogInformation($"Added {newProducts} new Products: {DateTimeOffset.Now}");
    }

    private void InitBasicUserData()
    {
        /* Nur bei leerer DB */

        const int users = 30;
        const int drinkActions = 8;
        const int challenges = 6;

        if (_dbContext.GetUsers().Count() < users)
        {
            _testDataHolder.InitUsers(users);
            _logger?.LogInformation("Added Testusers at: {time}", DateTimeOffset.Now);
        }

        if (_dbContext.GetDrinkActions().Count() < drinkActions)
        {
            _testDataHolder.InitDrinkActions(drinkActions);
            _logger?.LogInformation("Added Testdrinkactions at: {time}", DateTimeOffset.Now);
        }

        if (_dbContext.GetChallenges().Count() < challenges)
        {
            _testDataHolder.InitChallenges();
            _logger?.LogInformation("Added Testchallenges at: {time}", DateTimeOffset.Now);
        }
    }
}
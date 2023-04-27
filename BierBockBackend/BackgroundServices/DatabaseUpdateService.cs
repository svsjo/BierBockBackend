using BierBockBackend.Data;
using DataStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BierBockBackend.BackgroundServices;

public class DatabaseUpdateService : BackgroundService
{
    private readonly ILogger<DatabaseUpdateService>? _logger;
    private readonly AppDatabaseContext _dbContext;
    private readonly OpenFoodFactsApi _foodFactsApi;
    private readonly IServiceScope scope;
    private readonly TestDataHolder _testDataHolder;

    public DatabaseUpdateService(IServiceScopeFactory serviceScopeFactory)
    {
        scope = serviceScopeFactory.CreateScope();
        this._dbContext = scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();
        _logger = scope.ServiceProvider.GetService<ILogger<DatabaseUpdateService>>();
        _foodFactsApi = new OpenFoodFactsApi();
        _testDataHolder = new TestDataHolder(_dbContext, _logger);
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
        var newProducts = 0;

        foreach (var product in products
                     .Where(product => _dbContext.GetProducts()
                         .All(x => x.Code != product.Code)))
        {
            _dbContext.AddProduct(product);
            newProducts++;
        }

        _logger.LogInformation($"Added {newProducts} new Products: {DateTimeOffset.Now}");
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
            _logger.LogInformation("Added Testusers at: {time}", DateTimeOffset.Now);
        }

        if (_dbContext.GetDrinkActions().Count() < drinkActions)
        {
            _testDataHolder.InitDrinkActions(drinkActions);
            _logger.LogInformation("Added Testdrinkactions at: {time}", DateTimeOffset.Now);
        }

        if (_dbContext.GetChallenges().Count() < challenges)
        {
            _testDataHolder.InitChallenges();
            _logger.LogInformation("Added Testchallenges at: {time}", DateTimeOffset.Now);
        }
    }
}
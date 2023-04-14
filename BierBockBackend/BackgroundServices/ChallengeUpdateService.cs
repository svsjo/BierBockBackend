namespace BierBockBackend.BackgroundServices;

public class ChallengeUpdateService : BackgroundService
{
    private readonly ILogger<ChallengeUpdateService> _logger;

    public ChallengeUpdateService(ILogger<ChallengeUpdateService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Scheduled task started at: {time}", DateTimeOffset.Now);

            EvaluateChallenges();
            RemoveChallenges();
            AddChallenges();

            await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
        }
    }

    private void EvaluateChallenges()
    {
        // Add Points (or not)
    }

    private void RemoveChallenges()
    {
        // Remove from User
    }

    private void AddChallenges()
    {
        // Add to User
    }
}
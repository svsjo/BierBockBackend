#region

using DataStorage.HelperClasses;
using Microsoft.EntityFrameworkCore;

#endregion

namespace DataStorage;

public class AppDatabaseContext : DbContext
{
    private readonly ChallengeValidtorSelector _challengeValidatorSelector = new();
    private DbSet<DrinkAction> DrinkActions { get; set; }
    private DbSet<User> Users { get; set; }
    private DbSet<Product> Products { get; set; }
    private DbSet<Challenge> Challenges { get; set; }

    //Add-Migration Name
    //Update-Database

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /* 1-n-Beziehung User zu DrinkAction */
        modelBuilder.Entity<DrinkAction>()
            .HasOne(x => x.User)
            .WithMany(x => x.AllDrinkingActions)
            .OnDelete(DeleteBehavior.NoAction);

        /* Einseitige 1-n-Beziehung User zu FavouriteBeer */
        modelBuilder.Entity<User>()
            .HasOne(x => x.FavouriteBeer)
            .WithMany(x => x.UsersHavingThisAsFavouriteBeer)
            .OnDelete(DeleteBehavior.NoAction);

        /* Einseitige 1-1-Beziehung DrinkAction zu FavouriteBeer */
        modelBuilder.Entity<DrinkAction>()
            .HasOne(x => x.Product)
            .WithMany(x => x.UsedInDrinkActions)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public IQueryable<Challenge> GetChallenges()
    {
        return Challenges
            .AsQueryable()
            .AsSplitQuery();
    }

    public void AddChallenge(Challenge entry)
    {
        Challenges.Add(entry);
        SaveChanges();
    }

    public void AddUser(User entry)
    {
        Users.Add(entry);
        SaveChanges();
    }

    public IQueryable<User> GetUsers()
    {
        return Users
            .AsQueryable()
            .Include(x => x.AllDrinkingActions)
            .ThenInclude(x => x.Product)
            .Include(x => x.AllDrinkingActions)
            .ThenInclude(x => x.Location)
            .Include(x => x.FavouriteBeer)
            .Include(x => x.Location)
            .AsSplitQuery();
    }

    public IQueryable<Product> GetProducts()
    {
        return Products.AsQueryable();
    }

    public void AddProduct(Product entry)
    {
        Products.Add(entry);
        SaveChanges();
    }

    public void AddProducts(IEnumerable<Product> entries)
    {
        Products.AddRange(entries);
        SaveChanges();
    }

    public IQueryable<DrinkAction> GetDrinkActions()
    {
        return DrinkActions
            .AsQueryable()
            .Include(x => x.Product)
            .Include(x => x.Location)
            .AsSplitQuery();
    }

    public void AddDrinkAction(DrinkAction entry)
    {
        DrinkActions.Add(entry);
        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //sqllocaldb.exe start
        options
            .UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=bierbockdb;Trusted_Connection=True;MultipleActiveResultSets=true");
    }

    public IEnumerable<(Challenge challenge, ChallengeProgress challengeProgress)>
        CalculateChallengeProgresses(User user)
    {
        var challenges = GetChallenges().ToList().Where(x => x.IsActive);
        return challenges.ToList().Select(x =>
        (
            challenge: x,
            challengeProgress: _challengeValidatorSelector.ValidateChallengeProgress(
                user.AllDrinkingActions.ToList().Where(da => da.Time >= x.StartDate && da.Time <= x.EndDate)
                    .ToList(),
                x.SearchString,
                x.NeededQuantity,
                x.ChallengeType)
        )).ToList();
    }

    public void InsertDrinkAction(User user, DrinkAction drinkAction)
    {
        var oldProgress = CalculateChallengeProgresses(user)
            .Where(x => x.challengeProgress.Done == x.challengeProgress.Total);


        user.AllDrinkingActions.Add(drinkAction);
        AddDrinkAction(drinkAction);

        var newProgress = CalculateChallengeProgresses(user)
            .Where(x => x.challengeProgress.Done == x.challengeProgress.Total);


        var diff = newProgress.Except(oldProgress);
        var newPoints = diff.Sum(x => x.challenge.PossiblePoints);
        user.Points += newPoints;
        Update(user);
        SaveChanges();
    }
}
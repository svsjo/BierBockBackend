#region

using BierBockBackend.Data;
using DataStorage.HelpRelations;
using Microsoft.EntityFrameworkCore;

#endregion

namespace DataStorage;

public class AppDatabaseContext : DbContext
{
    private DbSet<ChallengePart> ChallengeParts { get; set; }
    private DbSet<Challenge> Challenges { get; set; }
    private DbSet<DrinkAction> DrinkActions { get; set; }
    private DbSet<User> Users { get; set; }
    private DbSet<Product> Products { get; set; }

    //Add-Migration Name
    //Update-Database

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /* n-m-Beziehung User zu Challenge */
        modelBuilder.Entity<ChallengeUser>()
            .HasOne(x => x.User)
            .WithMany(x => x.UserChallenges)
            .HasForeignKey(x => x.ChallengeId);

        modelBuilder.Entity<ChallengeUser>()
            .HasOne(x => x.Challenge)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.UserId);

        /* n-m-Beziehung Challenge zu ChallengePart */
        modelBuilder.Entity<ChallengePartChallenge>()
            .HasOne(x => x.Challenge)
            .WithMany(x => x.PartialChallenges)
            .HasForeignKey(x => x.ChallengePartId);

        modelBuilder.Entity<ChallengePartChallenge>()
            .HasOne(x => x.ChallengePart)
            .WithMany(x => x.Challenges)
            .HasForeignKey(x => x.ChallengeId);

        /* 1-n-Beziehung User zu DrinkAction */
        modelBuilder.Entity<DrinkAction>()
            .HasOne(x => x.User)
            .WithMany(x => x.AllDrinkingActions).OnDelete(DeleteBehavior.NoAction);
        // .HasForeignKey(x => x.UserId);

        /* Einseitige 1-n-Beziehung ChallengePart zu FavouriteBeer */
        modelBuilder.Entity<ChallengePart>()
            .HasOne(x => x.Beer)
            .WithMany(x => x.ChallengeParts).OnDelete(DeleteBehavior.NoAction);
        //.WithOne()
        //.HasForeignKey<ChallengePart>(x => x.BeerId);

        /* Einseitige 1-n-Beziehung User zu FavouriteBeer */
        modelBuilder.Entity<User>()
            .HasOne(x => x.FavouriteBeer)
            .WithMany(x => x.UsersHavingThisAsFavouriteBeer).OnDelete(DeleteBehavior.NoAction);
        //.WithOne()
        //.HasForeignKey<User>(x => x.BeerId);

        /* Einseitige 1-1-Beziehung DrinkAction zu FavouriteBeer */
        modelBuilder.Entity<DrinkAction>()
            .HasOne(x => x.Product)
            .WithMany(x => x.DrinkActions).OnDelete(DeleteBehavior.NoAction);
        //.WithOne()
        //.HasForeignKey<DrinkAction>(x => x.ProductId);
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
            .Include(x => x.UserChallenges)
            .ThenInclude(x => x.Challenge)
            .Include(x => x.AllDrinkingActions)
            .ThenInclude(x => x.Product)
            .Include(x => x.FavouriteBeer)
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

    public IQueryable<Challenge> GetChallenge()
    {
        return Challenges
            .AsQueryable()
            .Include(x => x.PartialChallenges)
            .ThenInclude(x => x.ChallengePart)
            .Include(x => x.Users) // kann raus?
            .ThenInclude(x => x.User) // kann raus?
            .AsSplitQuery();
    }

    public void AddChallenge(Challenge entry)
    {
        Challenges.Add(entry);
        SaveChanges();
    }

    public IQueryable<DrinkAction> GetDrinkActions()
    {
        return DrinkActions
            .AsQueryable()
            .Include(x => x.Product)
            .AsSplitQuery();
    }

    public void AddDrinkAction(DrinkAction entry)
    {
        DrinkActions.Add(entry);
        SaveChanges();
    }

    public IQueryable<ChallengePart> GetChallengeParts()
    {
        return ChallengeParts
            .AsQueryable()
            .Include(x => x.Challenges) // kann raus?
            .Include(x => x.Beer)
            .AsSplitQuery();
    }

    public void AddChallengePart(ChallengePart entry)
    {
        ChallengeParts.Add(entry);
        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //sqllocaldb.exe start
        options
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=bierbockdb;Trusted_Connection=True;MultipleActiveResultSets=true")
            .EnableDetailedErrors(true); // Später wieder entfernen wegen Performance

    }
}
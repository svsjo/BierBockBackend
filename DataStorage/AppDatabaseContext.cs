#region

using BierBockBackend.Data;
using DataStorage.HelpRelations;
using Microsoft.EntityFrameworkCore;

#endregion

namespace DataStorage;

public class AppDatabaseContext : DbContext
{
    public DbSet<ChallengePart> ChallengeParts { get; set; }
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<DrinkAction> DrinkActions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

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
            .WithMany(x => x.AllDrinkingActions)
            .HasForeignKey(x => x.UserId);
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
            .Include(x => x.AllDrinkingActions)
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

    public IQueryable<Challenge> GetChallenge()
    {
        return Challenges
            .AsQueryable()
            .Include(x => x.PartialChallenges)
            .Include(x => x.Users)
            .AsSplitQuery();
    }

    public void AddChallenge(Challenge entry)
    {
        Challenges.Add(entry);
        SaveChanges();
    }

    public IQueryable<DrinkAction> GetDrinkActions()
    {
        return DrinkActions.AsQueryable();
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
            .Include(x => x.Challenges)
            .AsSplitQuery();
    }

    public void AddChallenge(ChallengePart entry)
    {
        ChallengeParts.Add(entry);
        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source=ConnectionStrings:DefaultConnection");
    }
}
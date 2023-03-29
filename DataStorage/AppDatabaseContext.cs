#region

using BierBockBackend.Data;
using Microsoft.EntityFrameworkCore;

#endregion

namespace DataStorage;

public class AppDatabaseContext : DbContext
{
    private DbSet<ChallengePart> _challengeParts;
    private DbSet<Challenge> _challenges;
    private DbSet<DrinkAction> _drinkActions;

    private DbSet<Product> _products;
    //Add-Migration Name
    //Update-Database

    private DbSet<User> _users;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasMany<Challenge>().WithMany(x => x.User);
    }

    public void AddUser(User entry)
    {
        _users.Add(entry);
        SaveChanges();
    }

    public IQueryable<User> GetUsers()
    {
        return _users.AsQueryable();
    }

    public IQueryable<Product> GetProducts()
    {
        return _products.AsQueryable();
    }

    public void AddProduct(Product entry)
    {
        _products.Add(entry);
        SaveChanges();
    }

    public IQueryable<Challenge> GetChallenge()
    {
        return _challenges.AsQueryable();
    }

    public void AddChallenge(Challenge entry)
    {
        _challenges.Add(entry);
        SaveChanges();
    }

    public IQueryable<DrinkAction> GetDrinkActions()
    {
        return _drinkActions.AsQueryable();
    }

    public void AddDrinkAction(DrinkAction entry)
    {
        _drinkActions.Add(entry);
        SaveChanges();
    }

    public IQueryable<ChallengePart> GetChallengeParts()
    {
        return _challengeParts.AsQueryable();
    }

    public void AddChallenge(ChallengePart entry)
    {
        _challengeParts.Add(entry);
        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source=ConnectionStrings:DefaultConnection");
    }
}
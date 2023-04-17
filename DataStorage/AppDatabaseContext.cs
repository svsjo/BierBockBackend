#region

using Microsoft.EntityFrameworkCore;

#endregion

namespace DataStorage;

public class AppDatabaseContext : DbContext
{
    private DbSet<DrinkAction> DrinkActions { get; set; }
    private DbSet<User> Users { get; set; }
    private DbSet<Product> Products { get; set; }

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

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //sqllocaldb.exe start
        options
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=bierbockdb;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
}
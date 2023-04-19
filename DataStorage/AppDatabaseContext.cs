#region
using Microsoft.EntityFrameworkCore;

#endregion

namespace DataStorage;

public class AppDatabaseContext : DbContext
{
    private DbSet<DrinkAction> DrinkActions { get; set; }
    private DbSet<User> Users { get; set; }
    private DbSet<Product> Products { get; set; }

    private Mutex _mutexDrinkActions = new Mutex();
    private Mutex _mutexUsers = new Mutex();
    private Mutex _mutexProducts = new Mutex();

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
        try
        {
            _mutexUsers.WaitOne();

            Users.Add(entry);
            SaveChanges();
        }
        finally
        {
            _mutexUsers.ReleaseMutex();
        }
    }

    public IQueryable<User> GetUsers()
    {
        try
        {
            _mutexUsers.WaitOne();

            return Users
                .AsQueryable()
                .Include(x => x.AllDrinkingActions)
                .ThenInclude(x => x.Product)
                .Include(x => x.FavouriteBeer)
                .AsSplitQuery();
        }
        finally
        {
            _mutexUsers.ReleaseMutex();
        }
    }

    public IQueryable<Product> GetProducts()
    {
        try
        {
            _mutexProducts.WaitOne();

            return Products.AsQueryable();
        }
        finally
        {
            _mutexProducts.ReleaseMutex();
        }
    }

    public void AddProduct(Product entry)
    {
        try
        {
            _mutexProducts.WaitOne();

            Products.Add(entry);
            SaveChanges();
        }
        finally
        {
            _mutexProducts.ReleaseMutex();
        }
    }

    public void AddProducts(IEnumerable<Product> entries)
    {
        try
        {
            _mutexProducts.WaitOne();

            Products.AddRange(entries);
            SaveChanges();
        }
        finally
        {
            _mutexProducts.ReleaseMutex();
        }
    }

    public IQueryable<DrinkAction> GetDrinkActions()
    {
        try
        {
            _mutexDrinkActions.WaitOne();

            return DrinkActions
                .AsQueryable()
                .Include(x => x.Product)
                .AsSplitQuery();
        }
        finally
        {
            _mutexDrinkActions.ReleaseMutex();
        }
    }

    public void AddDrinkAction(DrinkAction entry)
    {
        try
        {
            _mutexDrinkActions.WaitOne();

            DrinkActions.Add(entry);
            SaveChanges();
        }
        finally
        {
            _mutexDrinkActions.ReleaseMutex();
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //sqllocaldb.exe start
        options
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=bierbockdb;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
}
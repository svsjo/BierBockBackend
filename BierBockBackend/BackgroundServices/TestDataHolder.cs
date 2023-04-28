using BierBockBackend.Auth;
using DataStorage;
using DataStorage.HelperClasses;

namespace BierBockBackend.BackgroundServices;

public class TestDataHolder
{
    private readonly ILogger<DatabaseUpdateService>? _logger;
    private readonly AppDatabaseContext _dbContext;

    private Coordinate _horberCoords = new Coordinate()
    {
        Latitude = 48.442078,
        Longitude = 8.68488512,
        Altitude = 421.0
    };

    private int _offsetMin = 2;
    private int _offsetMax = 100;

    public TestDataHolder(AppDatabaseContext dbContext, ILogger<DatabaseUpdateService>? logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    #region Horber Init

    public void InitHorberDrinkActions(int numberDrinkActions)
    {
        var products = _dbContext.GetProducts().Take(numberDrinkActions).ToList();

        var random = new Random();
        var users = _dbContext.GetUsers().ToList();

        foreach (var product in products)
        {
            foreach (var user in users)
            {
                var drinkAction = new DrinkAction
                {
                    Product = product,
                    Time = DateTime.Now.AddMinutes(5),
                    Location = new Coordinate()
                    {
                        Latitude = _horberCoords.Latitude + random.Next(_offsetMin, _offsetMax)/1000.0,
                        Longitude = _horberCoords.Longitude + random.Next(_offsetMin, _offsetMax) / 1000.0,
                        Altitude = _horberCoords.Altitude + random.Next(-10, 30),
                    },
                    User = user,
                };

                _dbContext.AddDrinkAction(drinkAction);
                user.AllDrinkingActions.Add(drinkAction);
                product.UsedInDrinkActions.Add(drinkAction);
            }
        }

        _dbContext.SaveChanges();
    }

    #endregion

    #region Normal Init

    public void InitDrinkActions(int numberDrinkActions)
    {
        var products = _dbContext.GetProducts().Take(numberDrinkActions).ToList();

        var random = new Random();
        var users = _dbContext.GetUsers().ToList();

        foreach (var product in products)
        {
            foreach (var user in users)
            {
                var drinkAction = new DrinkAction
                {
                    Product = product,
                    Time = DateTime.Now.AddMinutes(5),
                    Location = new Coordinate()
                    {
                        Latitude = 0 + random.Next(-90, 90),
                        Longitude = 0 + random.Next(-180, 180),
                        Altitude = 500 + random.Next(-100, 100),
                    },
                    User = user,
                };

                _dbContext.AddDrinkAction(drinkAction);
                user.AllDrinkingActions.Add(drinkAction);
                product.UsedInDrinkActions.Add(drinkAction);
            }
        }

        _dbContext.SaveChanges();
    }

    public void InitUsers(int numberUsers)
    {
        var products = _dbContext.GetProducts().Take(numberUsers).ToList();

        var hash = PasswordHashing.HashPassword("Password123");

        #region mustimax

        var user = new User
        {
            Name = "Mustermann",
            VorName = "Max",
            UserName = "mustimax",
            PasswordHash = hash.Hash,
            PasswordSalt = hash.Salt,
            Email = "max.mustermann@example.com",
            FavouriteBeer = products.ElementAt(0),
            BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
            Points = 10,
            EmailConfirmed = true,
            Location = new Coordinate()
            {
                Latitude = 48.1351,
                Longitude = 11.5820,
                Altitude = 600
            }
        };

        _dbContext.AddUser(user);
        products.ElementAt(0).UsersHavingThisAsFavouriteBeer.Add(user);

        var user77 = new User
        {
            Name = "Mustermann",
            VorName = "Max",
            UserName = "maximust",
            PasswordHash = hash.Hash,
            PasswordSalt = hash.Salt,
            Email = "mustermann.max@example.com",
            FavouriteBeer = products.ElementAt(0),
            BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
            Points = 500,
            EmailConfirmed = true,
            Location = new Coordinate()
            {
                Latitude = 48.1351,
                Longitude = 11.5820,
                Altitude = 600
            }
        };

        _dbContext.AddUser(user77);
        products.ElementAt(0).UsersHavingThisAsFavouriteBeer.Add(user77);

        #endregion

        #region admin

        var admin = new User
        {
            Name = "Strator",
            VorName = "Admin",
            UserName = "admin",
            PasswordHash = hash.Hash,
            PasswordSalt = hash.Salt,
            Email = "max.mustermann@example.com",
            FavouriteBeer = products.ElementAt(0),
            BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
            Points = 10,
            EmailConfirmed = true,
            IsAdmin = true,
            Location = new Coordinate()
            {
                Latitude = 48.1351,
                Longitude = 11.5820,
                Altitude = 600
            }
        };

        _dbContext.AddUser(admin);
        products.ElementAt(0).UsersHavingThisAsFavouriteBeer.Add(admin);

        #endregion

        var random = new Random();

        foreach (var product in products)
        {
            var vorname = Guid.NewGuid().ToString();
            var nachname = Guid.NewGuid().ToString();
            var username = vorname[..7] + nachname[..7];
            var mail = username + "@example.com";

            var user2 = new User
            {
                Name = vorname,
                VorName = nachname,
                UserName = username,
                PasswordHash = hash.Hash,
                PasswordSalt = hash.Salt,
                Email = mail,
                FavouriteBeer = product,
                BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
                Points = 0 + random.Next(0, 500),
                EmailConfirmed = true,
                Location = new Coordinate()
                {
                    Latitude = 0 + random.Next(-90, 90),
                    Longitude = 0 + random.Next(-180, 180),
                    Altitude = 500 + random.Next(-100, 100),
                }
            };

            _dbContext.AddUser(user2);
            product.UsersHavingThisAsFavouriteBeer.Add(user2);
        }

        _dbContext.SaveChanges();
    }

    public void InitChallenges()
    {
        var challenge = new Challenge()
        {
            ChallengeType = ChallengeType.DifferentBrand,
            Description = "Trinke von drei unterschiedlichen Marken",
            PossiblePoints = 30,
            NeededQuantity = 3
        };

        var challenge2 = new Challenge()
        {
            ChallengeType = ChallengeType.SameBrand,
            Description = "Trinke drei Bier der Marke Alpirsbacher",
            SearchString = "Alpirsbacher",
            PossiblePoints = 50,
            NeededQuantity = 3
        };

        var challenge3 = new Challenge()
        {
            ChallengeType = ChallengeType.DifferentBeer,
            Description = "Trinke fünf unterschiedliche Bier",
            PossiblePoints = 20,
            NeededQuantity = 5
        };

        var challenge4 = new Challenge()
        {
            ChallengeType = ChallengeType.SameBeer,
            Description = "Trinke fünf Alpirsbacher Spezial",
            SearchString = "Alpirsbacher Spezial",
            PossiblePoints = 30,
            NeededQuantity = 5
        };

        var challenge5 = new Challenge()
        {
            ChallengeType = ChallengeType.DifferentSize,
            Description = "Trinke drei Biere unterschiedlicher Größe",
            PossiblePoints = 20,
            NeededQuantity = 3
        };

        var challenge6 = new Challenge()
        {
            ChallengeType = ChallengeType.SameSize,
            Description = "Trinke drei Bier der Größe 0,5L",
            SearchString = "0,5L",
            PossiblePoints = 50,
            NeededQuantity = 3
        };

        _dbContext.AddChallenge(challenge);
        _dbContext.AddChallenge(challenge2);
        _dbContext.AddChallenge(challenge3);
        _dbContext.AddChallenge(challenge4);
        _dbContext.AddChallenge(challenge5);
        _dbContext.AddChallenge(challenge6);
    }

    #endregion
}
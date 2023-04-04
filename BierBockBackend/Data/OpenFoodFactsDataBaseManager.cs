using DataStorage;
using DataStorage.HelpRelations;

namespace BierBockBackend.Data
{
    public class OpenFoodFactsDataBaseManager
    {
        private readonly AppDatabaseContext _dbContext;

        public OpenFoodFactsDataBaseManager(AppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Insert()
        {
            var productsTask = new OpenFoodFactsApi().GetBeerData();
            var products = productsTask.Result;
            //_dbContext.AddProducts(products);
            foreach (var product in products)
            {
                if (_dbContext.GetProducts().All(x => x.Code != product.Code))
                {
                    _dbContext.AddProduct(product);
                }
            }
        }

        public void InitBasicUserData()
        {
            if (!_dbContext.GetUsers().Any()) /* Nur bei leerer DB */
            {

                var user = new User
                {
                    Token = "123456",
                    Name = "Max Mustermann",
                    PasswordHash = "Password123",
                    Email = "max.mustermann@example.com",
                    FavouriteBeer = _dbContext.GetProducts().First(),
                    BirthDate = new DateOnly(1990, 1, 1).ToLongDateString(),
                    Points = 10,
                    Location = new Coordinate()
                    {
                        Latitude = 48.1351,
                        Longitude = 11.5820,
                        Altitude = 100
                    }
                };

                _dbContext.AddUser(user);

                var drinkAction = new DrinkAction
                {
                    Product = _dbContext.GetProducts().First(),
                    Time = DateTime.Now,
                    Location = new Coordinate()
                    {
                        Latitude = 48.1351,
                        Longitude = 11.5820,
                        Altitude = 100,
                    },
                    User = _dbContext.GetUsers().First(),
                };

                _dbContext.AddDrinkAction(drinkAction);

                _dbContext.GetUsers().First().AllDrinkingActions.Add(drinkAction);
                _dbContext.SaveChanges();

                var challengePart = new ChallengePart
                {
                    Description = "Trink ein Bier",
                    Beer = _dbContext.GetProducts().First(),
                    Quantity = 1,
                };

                _dbContext.AddChallengePart(challengePart);

                var challenge = new Challenge
                {
                    PossiblePoints = 50,
                    Description = "Trink jeden Tag ein Bier",
                    StartDate = DateTime.Now.AddDays(-7),
                    EndDate = DateTime.Now.AddDays(7)
                };

                _dbContext.AddChallenge(challenge);

                var challengePartAssignment = new ChallengePartChallenge()
                {
                    Challenge = _dbContext.GetChallenge().First(),
                    ChallengePart = _dbContext.GetChallengeParts().First(),
                };

                _dbContext.GetChallenge().First().PartialChallenges.Add(challengePartAssignment);
                _dbContext.GetChallengeParts().First().Challenges.Add(challengePartAssignment);
                _dbContext.SaveChanges();

                var challengeUser = new ChallengeUser
                {
                    User = _dbContext.GetUsers().First(),
                    Challenge = _dbContext.GetChallenge().First()
                };

                _dbContext.GetUsers().First().UserChallenges.Add(challengeUser);
                _dbContext.GetChallenge().First().Users.Add(challengeUser);
                _dbContext.SaveChanges();

                _dbContext.GetProducts().First().UsedInChallengeParts.Add(challengePart);
                _dbContext.GetProducts().First().UsedInDrinkActions.Add(drinkAction);
                _dbContext.GetProducts().First().UsersHavingThisAsFavouriteBeer.Add(user);
                _dbContext.SaveChanges();
            }
        }
    }
}

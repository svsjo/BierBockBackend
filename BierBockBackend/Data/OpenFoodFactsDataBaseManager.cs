using DataStorage;

namespace BierBockBackend.Data
{
    public class OpenFoodFactsDataBaseManager
    {
        private readonly AppDatabaseContext _dbContext;

        public OpenFoodFactsDataBaseManager(AppDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async void Insert()
        {
            var productsTask = new OpenFoodFactsApi().GetBeerData();
            var products = await productsTask;
            _dbContext.AddProducts(products);
        }
    }
}

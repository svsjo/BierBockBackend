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
            //_dbContext.AddProducts(products);
            foreach (var product in products)
            {
                if (_dbContext.GetProducts().All(x => x.Code != product.Code))
                {
                    _dbContext.AddProduct(product);
                }
            }
        }
        }
}

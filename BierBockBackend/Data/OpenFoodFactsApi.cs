using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBockBackend.Data
{
    public class OpenFoodFactsApi
    {
        public static async Task InitializeQuery()
        {
            var openFoodFactsApi = new OpenFoodFactsApi();
            var allProducts = await openFoodFactsApi.GetAllProducts();
            Console.WriteLine($"Alle Produkte: {allProducts.Count}");
            var beers = FilterBeers(allProducts).ToList();
            Console.WriteLine($"Nur Biere: {beers.Count}");
        }

        private static IEnumerable<Product> FilterBeers(IEnumerable<Product> allProducts)
        {
            var searchStrings = new List<string>()
            {
                "pilsener",
                "bier",
                "hefeweizen",
                "beer",
            };

            var categoryFilter = allProducts.Where(x => x.Categories != null && searchStrings.Any(y => x.Categories.ToLower().Contains(y)));

            return categoryFilter;
        }

        private async Task<List<Product>> GetAllProducts()
        {
            var allProducts = new List<Product>();
            var page = 1;
            const int pageSize = 500;

            while (true)
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://world.openfoodfacts.org/cgi/search.pl?action=process&sort_by=unique_scans_n&page={page}&page_size={pageSize}&json=true");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<ProductList>(result);

                    if (products?.Products == null) break; /* Keine weiteren Produkte existieren == alle Produkte abgefragt */

                    allProducts.AddRange(products.Products);

                    if (products.Products.Count < pageSize) /* Weniger Ergebnisse als Seitengröße == keine weiteren Produkte existieren == alle Produkte abgefragt */
                        break;
                    else
                        page++;
                }
                else /* Kein erfolgreicher Request == Seite existiert nicht == alle Produkte abgefragt */
                {
                    break;
                }
            }

            return allProducts;
        }
    }
}

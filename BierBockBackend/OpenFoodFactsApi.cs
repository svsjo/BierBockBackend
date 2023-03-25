using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFoodDbAbfrage
{
    public class OpenFoodFactsApi
    {
        public static async Task InitializeQuery()
        {
            var openFoodFactsApi = new OpenFoodFactsApi();
            var allProducts = await openFoodFactsApi.GetAllProducts();
            var beers = FilterBeers(allProducts).ToList();
            Console.WriteLine(allProducts.Count);
            Console.WriteLine(beers.Count);
        }

        private static IEnumerable<Product> FilterBeers(IEnumerable<Product> allProducts)
        {
            var searchStrings = new List<string>()
            {
                "Bier",
                "Pilsener",
                "bier",
                "Hefeweizen",
                "Beer",
                "Weizen",
                "Weißbier",
                "Pilsener",
                "beer", // ggf. mehr ausländische Namen
            };

            var categoryFilter = allProducts.Where(x => x.Categories != null && searchStrings.Any(y => x.Categories.Contains(y)));

            return categoryFilter;
        }

        private async Task<List<Product>> GetAllProducts()
        {
            var allProducts = new List<Product>();
            var page = 1;
            const int pageSize = 20;

            while (true)
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://world.openfoodfacts.org/cgi/search.pl?action=process&sort_by=unique_scans_n&page={page}&page_size={pageSize}&json=true");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<RootObject>(result);
                    allProducts.AddRange(products.Products);
                    if (products.Products.Count < pageSize) // check if it is the last page
                        break;
                    else
                        page++;
                }
                else
                {
                    break;
                }
            }

            return allProducts;
        }
    }
}

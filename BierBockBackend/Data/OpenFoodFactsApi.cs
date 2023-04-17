using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorage;

namespace BierBockBackend.Data
{
    public class OpenFoodFactsApi
    {
        public async Task<List<Product>> GetBeerData()
        {
            return await GetAllBeers();
        }

        private async Task<List<Product>> GetAllBeers()
        {
            var allProducts = new List<Product>();
            var page = 1;
            const int pageSize = 1000;
            const string category = "beers";

            while (true)
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://world.openfoodfacts.org/cgi/search.pl?action=process&tagtype_0=categories&tag_contains_0=contains&tag_0={category}&page={page}&page_size={pageSize}&json=true");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<ProductList>(result);

                    if (products?.Products == null) break;

                    allProducts.AddRange(products.Products);

                    if (products.Products.Count < pageSize) // check if it is the last page
                        break;
                    else
                    {
                        page++;
                        Console.WriteLine($"++500 -> Gesamt: {allProducts.Count}");
                    }
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
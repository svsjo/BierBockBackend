#region

using System.Text.Json;
using DataStorage;

#endregion

namespace BierBockBackend.Data;

public class OpenFoodFactsClient
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
            var response = await httpClient.GetAsync(
                $"https://world.openfoodfacts.org/cgi/search.pl?action=process&tagtype_0=categories&tag_contains_0=contains&tag_0={category}&page={page}&page_size={pageSize}&json=true");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<ProductList>(result);

                if (products?.Products == null) break;

                allProducts.AddRange(products.Products);

                if (products.Products.Count < pageSize) // check if it is the last page
                {
                    break;
                }

                page++;
                Console.WriteLine($"++1000 -> Gesamt: {allProducts.Count}");
            }
            else /* Kein erfolgreicher Request == Seite existiert nicht == alle Produkte abgefragt */
            {
                break;
            }
        }

        return allProducts;
    }
}
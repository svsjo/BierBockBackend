using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BierBockBackend.Data;

public class Product
{
    #region Allgemeine Infos

    /* Primärschlüssel */
    [JsonProperty("code")]
    [Key] public string Code { get; set; }

    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    [JsonProperty("brands")]
    public string? Brands { get; set; }

    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }

    [JsonProperty("categories")]
    public string? Categories { get; set; }

    [JsonProperty("quantity")]
    public string? Quantity { get; set; }

    [JsonProperty("generic_name")]
    public string? GenericName { get; set; }

    #endregion


    #region Inhaltsstoffe

    [JsonProperty("ingredients_text")]
    public string? IngredientsText { get; set; }

    [JsonProperty("nutriscore_grade")]
    public string? NutriscoreGrade { get; set; }

    [JsonProperty("energy-kcal_100g")]
    public decimal? EnergyKcalPer100g { get; set; }

    [JsonProperty("fat_100g")]
    public decimal? FatPer100g { get; set; }

    [JsonProperty("saturated-fat_100g")]
    public decimal? SaturatedFatPer100g { get; set; }

    [JsonProperty("carbohydrates_100g")]
    public decimal? CarbohydratesPer100g { get; set; }

    [JsonProperty("sugars_100g")]
    public decimal? SugarsPer100g { get; set; }

    [JsonProperty("fiber_100g")]
    public decimal? FiberPer100g { get; set; }

    [JsonProperty("proteins_100g")]
    public decimal? ProteinsPer100g { get; set; }

    [JsonProperty("salt_100g")]
    public decimal? SaltPer100g { get; set; }

    [JsonProperty("sodium_100g")]
    public decimal? SodiumPer100g { get; set; }

    #endregion


    #region Bierspezifisch

    [JsonProperty("alcohol_100g")]
    public decimal? AlcoholByVolume { get; set; }

    [JsonProperty("brewing_method")]
    public string? BrewingMethod { get; set; }

    [JsonProperty("fermentation")]
    public string? Fermentation { get; set; }

    [JsonProperty("color")]
    public string? Color { get; set; }

    [JsonProperty("ibu")]
    public int? IBU { get; set; }

    [JsonProperty("hops")]
    public string? Hops { get; set; }

    [JsonProperty("yeast")]
    public string? Yeast { get; set; }

    [JsonProperty("malt")]
    public string? Malt { get; set; }

    [JsonProperty("beer_style")]
    public string? BeerStyle { get; set; }

    [JsonProperty("beer_type")]
    public string? BeerType { get; set; }

    [JsonProperty("carbonation_level")]
    public string? CarbonationLevel { get; set; }

    [JsonProperty("taste")]
    public string? Taste { get; set; }

    #endregion
}

public class ProductList
{
    [JsonProperty("products")]
    public ICollection<Product>? Products { get; set; }
}
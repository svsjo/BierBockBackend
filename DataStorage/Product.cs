using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataStorage;

public class Product
{
    [JsonIgnore] public virtual ICollection<User> UsersHavingThisAsFavouriteBeer { get; set; }

    [JsonIgnore] public virtual ICollection<DrinkAction> UsedInDrinkActions { get; set; }

    #region Allgemeine Infos

    [Newtonsoft.Json.JsonIgnore] [Key] public int Id { get; set; }

    [JsonPropertyName("code")] public string Code { get; set; } = string.Empty;

    [JsonPropertyName("product_name")] public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("brands")] public string? Brands { get; set; } = string.Empty;

    [JsonPropertyName("image_url")] public string? ImageUrl { get; set; } = string.Empty;

    [JsonPropertyName("categories")] public string? Categories { get; set; } = string.Empty;

    [JsonPropertyName("quantity")] public string? Quantity { get; set; } = string.Empty;

    [JsonPropertyName("generic_name")] public string? GenericName { get; set; } = string.Empty;

    #endregion


    #region Inhaltsstoffe

    [JsonPropertyName("ingredients_text")] public string? IngredientsText { get; set; } = string.Empty;

    [JsonPropertyName("nutriscore_grade")] public string? NutriscoreGrade { get; set; } = string.Empty;

    [JsonPropertyName("energy-kcal_100g")] public decimal? EnergyKcalPer100g { get; set; }

    [JsonPropertyName("fat_100g")] public decimal? FatPer100g { get; set; }

    [JsonPropertyName("saturated-fat_100g")]
    public decimal? SaturatedFatPer100g { get; set; }

    [JsonPropertyName("carbohydrates_100g")]
    public decimal? CarbohydratesPer100g { get; set; }

    [JsonPropertyName("sugars_100g")] public decimal? SugarsPer100g { get; set; }

    [JsonPropertyName("fiber_100g")] public decimal? FiberPer100g { get; set; }

    [JsonPropertyName("proteins_100g")] public decimal? ProteinsPer100g { get; set; }

    [JsonPropertyName("salt_100g")] public decimal? SaltPer100g { get; set; }

    [JsonPropertyName("sodium_100g")] public decimal? SodiumPer100g { get; set; }

    #endregion


    #region Bierspezifisch

    [JsonPropertyName("alcohol_100g")] public decimal? AlcoholByVolume { get; set; }

    [JsonPropertyName("brewing_method")] public string? BrewingMethod { get; set; } = string.Empty;

    [JsonPropertyName("fermentation")] public string? Fermentation { get; set; } = string.Empty;

    [JsonPropertyName("color")] public string? Color { get; set; } = string.Empty;

    [JsonPropertyName("ibu")] public int? IBU { get; set; }

    [JsonPropertyName("hops")] public string? Hops { get; set; } = string.Empty;

    [JsonPropertyName("yeast")] public string? Yeast { get; set; } = string.Empty;

    [JsonPropertyName("malt")] public string? Malt { get; set; } = string.Empty;

    [JsonPropertyName("beer_style")] public string? BeerStyle { get; set; } = string.Empty;

    [JsonPropertyName("beer_type")] public string? BeerType { get; set; } = string.Empty;

    [JsonPropertyName("carbonation_level")]
    public string? CarbonationLevel { get; set; } = string.Empty;

    [JsonPropertyName("taste")] public string? Taste { get; set; } = string.Empty;

    #endregion
}

public class ProductList
{
    [JsonPropertyName("products")] public ICollection<Product>? Products { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace BierBockBackend.Data;

public class PartialChallenge
{
    [Key] public int Id { get; set; }

    public string BeerCode { get; }

    public int Quantity { get; set; }
}
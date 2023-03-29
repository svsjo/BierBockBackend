using System.ComponentModel.DataAnnotations;

namespace BierBockBackend.Data;

public class ChallengePart
{
    [Key] public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public string BeerCode { get; }

    public int Quantity { get; set; }

    public virtual IQueryable<Challenge> Challenges { get; set; }
}
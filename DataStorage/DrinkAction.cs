using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataStorage;

public class DrinkAction
{
    [JsonIgnore] [Key] public int Id { get; set; }

    public DateTime Time { get; set; } = DateTime.Now;

    public Coordinate Location { get; set; }

    [JsonIgnore] [ForeignKey("UserId")] public virtual User User { get; set; }

    [JsonIgnore] [ForeignKey("ProductId")] public virtual Product Product { get; set; }
}
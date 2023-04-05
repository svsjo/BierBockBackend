using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoCoordinatePortable;
using Newtonsoft.Json;

namespace BierBockBackend.Data
{
    public class DrinkAction
    {
        [Key] public int Id { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public Coordinate Location { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}

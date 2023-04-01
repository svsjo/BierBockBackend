using System.ComponentModel.DataAnnotations;
using GeoCoordinatePortable;

namespace BierBockBackend.Data
{
    public class DrinkAction
    {
        [Key] public int Id { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public Coordinate Location { get; set; }


        /* Fremdschlüssel */
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}

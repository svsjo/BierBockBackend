using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoCoordinatePortable;

namespace BierBockBackend.Data
{
    public class DrinkAction
    {
        [Key] public int Id { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public Coordinate Location { get; set; }


        /* Fremdschlüssel */
        [ForeignKey("UserId")]
        //public int UserId { get; set; }
        public virtual User User { get; set; }


        [ForeignKey("ProductId")]
        //public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using GeoCoordinatePortable;

namespace BierBockBackend.Data
{
    public class DrinkAction
    {
        [Key] public int Id { get; set; }

        public string BeerCode { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public GeoCoordinate GetCoordinate => new GeoCoordinate(this.Latitude, this.Longitude, this.Altitude);

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        /* Fremdschlüssel */
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}

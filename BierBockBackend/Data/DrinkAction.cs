using System.ComponentModel.DataAnnotations;

namespace BierBockBackend.Data
{
    public class DrinkAction
    {
        [Key] public int Id { get; set; }

        public string BeerCode { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;
    }
}

namespace BierBockBackend.Data
{
    public class DrinkAction
    {
        public DrinkAction(string beerCode)
        {
            BeerCode = beerCode;
            DateTime = DateTime.Now;
        }

        /* Fremdschlüssel: Primärschlüssel von Produkt */
        public string BeerCode { get; }

        public DateTime DateTime { get; }
    }
}

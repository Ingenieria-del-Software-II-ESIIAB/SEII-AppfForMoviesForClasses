namespace AppForSEII2526.API.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }


        public Purchase Purchase { get; set; }

         public Movie Movie { get; set; }
    }
}

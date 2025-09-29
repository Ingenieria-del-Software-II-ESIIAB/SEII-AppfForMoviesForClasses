namespace AppForSEII2526.API.Models
{
    public class RentalItem
    {
        public int Id { get; set; }
        public Rental Rental { get; set; }

        [Precision(10,2)]
        public decimal Price { get; set; }
  
    }
}

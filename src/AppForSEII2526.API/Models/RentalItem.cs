namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(RentalId), nameof(MovieId))]
    public class RentalItem
    {
        public int RentalId { get; set; }

        [ForeignKey(nameof(RentalId))]
        public Rental Rental { get; set; }

        [Precision(10,2)]
        public decimal Price { get; set; }

        public int MovieId { get; set; }

        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; }


        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string? Description { get; set; }

    }
}

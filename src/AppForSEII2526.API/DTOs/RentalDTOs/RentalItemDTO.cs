namespace AppForSEII2526.API.DTOs.RentalDTOs {
    public class RentalItemDTO {
        public RentalItemDTO(int movieID, string title, string genre, decimal priceForRenting, string? description) {
            MovieID = movieID;
            Title = title;
            PriceForRenting = priceForRenting;
            Description = description;
            Genre = genre;
        }

        public int MovieID { get; set; }


        public string Title { get; set; }

        [Precision(10, 2)]
        public decimal PriceForRenting { get; set; }

        public string? Description { get; set; }

        public string Genre { get; set; }
    }
}

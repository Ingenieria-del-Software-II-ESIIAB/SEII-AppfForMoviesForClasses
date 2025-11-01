namespace AppForSEII2526.API.DTOs.MovieDTOs {
    public class MovieForRentalDTO {
        public MovieForRentalDTO(int id, string title, string genreName, DateTime releaseDate, decimal priceForRenting, DateTime? lastRental) {
            Id = id;
            Title = title;
            Genre= genreName;
            ReleaseDate = releaseDate;
            PriceForRenting = priceForRenting;
            LastRental = lastRental;
        }

        public int Id { get; set; }


        [StringLength(50, ErrorMessage = "Title name cannot be longer than 50 characters.")]
        public string Title { get; set; }

        public string Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Precision(10, 2)]
        public decimal PriceForRenting { get; set; } 

        public DateTime? LastRental { get; set; }

        public override bool Equals(object? obj) {
            return obj is MovieForRentalDTO dTO &&
                   Id == dTO.Id &&
                   Title == dTO.Title &&
                   Genre == dTO.Genre &&
                   ReleaseDate == dTO.ReleaseDate &&
                   PriceForRenting == dTO.PriceForRenting &&
                   LastRental == dTO.LastRental;
        }
    }
}

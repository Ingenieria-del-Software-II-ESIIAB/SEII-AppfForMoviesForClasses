namespace AppForSEII2526.API.DTOs.MovieDTOs {
    public class MovieForPurchaseDTO {
        public MovieForPurchaseDTO(int id, string title, string genre, DateTime releaseDate, decimal priceForPurchase) {
            Id = id;
            Title = title;
            Genre = genre;
            ReleaseDate = releaseDate;
            PriceForPurchase = priceForPurchase;
        }

        public int Id { get; set; }


        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        //it assigns a value by default
        public string Title { get; set; } = string.Empty;


        public string Genre { get; set; }

        public DateTime ReleaseDate { get; set; }



        [Range(1, float.MaxValue, ErrorMessage = "Minimum price is 1 ")]
        [Precision(10, 2)]
        public decimal PriceForPurchase { get; set; }

        public override bool Equals(object? obj) {
            return obj is MovieForPurchaseDTO dTO &&
                   Id == dTO.Id &&
                   Title == dTO.Title &&
                   Genre == dTO.Genre &&
                   ReleaseDate == dTO.ReleaseDate &&
                   PriceForPurchase == dTO.PriceForPurchase;
        }
    }
}

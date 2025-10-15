namespace AppForSEII2526.API.DTOs.MovieDTOs {
    public class MovieForRentalDTO {
        public MovieForRentalDTO(int id, string title, string genreName) {
            Id = id;
            Title = title;
            Genre= genreName;
        }

        public int Id { get; set; }


        [StringLength(50, ErrorMessage = "Title name cannot be longer than 50 characters.")]
        public string Title { get; set; }

        public string Genre { get; set; }
    }
}

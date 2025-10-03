namespace AppForSEII2526.API.Models {
    public class Movie {

        public int Id { get; set; }


        [StringLength(50, ErrorMessage = "Title name cannot be longer than 50 characters.")]
        public string Title { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Display(Name = "Price For Purchase")]
        [Precision(10, 2)]
        public decimal PriceForPurchase { get; set; }

        [Display(Name = "Quantity For Purchase")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum quantity for Purchase is 1")]
        public int QuantityForPurchase { get; set; }


        public Genre Genre { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Display(Name = "Price For Renting")]
        [Precision(10, 2)]
        public decimal PriceForRenting { get; set; }

        [Display(Name = "Quantity For Renting")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public int QuantityForRental { get; set; }
    }
}

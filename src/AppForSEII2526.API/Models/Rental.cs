namespace AppForSEII2526.API.Models {
    public class Rental {
        public int Id { get; set; }

        [StringLength(20,
            ErrorMessage ="Maximum 20, minimum 10",
            MinimumLength =10)]
        public string Title { get; set; }

        [Precision(10,2)]
        public decimal CostofRental { get; set; }
    }
}

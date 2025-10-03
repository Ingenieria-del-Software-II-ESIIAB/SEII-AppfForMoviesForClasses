namespace AppForSEII2526.API.Models {
    public class Rental {
        public int Id { get; set; }

        [StringLength(20,ErrorMessage ="Maximum 50, minimum 10",MinimumLength =10)]
        public string DeliveryAddress { get; set; }

        [StringLength(20,ErrorMessage = "Maximum 50, minimum 10",MinimumLength = 10)]
        public string NameCustomer { get; set; }

        [StringLength(20,ErrorMessage = "Maximum 50, minimum 10",MinimumLength = 10)]
        public string SurnameCustomer { get; set; }

        [Precision(10,2)]
        public decimal CostofRental { get; set; }

        public IList<RentalItem> RentalItems { get; set; }

        public DateTime RentalDate { get; set; }

        public DateTime RentalDateFrom { get; set; }
        public DateTime RentalDateTo { get; set; }

        public PaymentMethodTypes PaymentMethod { get; set; }

        public ApplicationUser Customer { get; set; }

    }


}

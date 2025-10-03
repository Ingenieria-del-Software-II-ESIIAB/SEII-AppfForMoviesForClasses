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

        public PaymentMethodType PaymentMethod { get; set; }


        //if the Customer is deleted the related Rental won't be deleted
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public ApplicationUser Customer { get; set; }


        //if we want to specify the foreign key
        //public string CustomerId { get; set; }

        //[ForeignKey(nameof(CustomerId))]
        //public ApplicationUser Customer { get; set; }

    }


}

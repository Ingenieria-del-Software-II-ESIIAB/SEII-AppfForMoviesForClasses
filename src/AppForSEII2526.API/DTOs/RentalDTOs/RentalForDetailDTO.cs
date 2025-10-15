using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.RentalDTOs {
    public class RentalForDetailDTO {
        public RentalForDetailDTO(int id, decimal costofRental, DateTime rentalDate,string deliveryAddress, 
                string nameCustomer, string surnameCustomer, 
                DateTime rentalDateFrom, DateTime rentalDateTo, PaymentMethodType paymentMethod, 
                string userNameCustomer, IList<RentalItemDTO> rentalItems) {

            Id = id;
            CostofRental = costofRental;
            RentalDate = rentalDate;
            DeliveryAddress = deliveryAddress;
            NameCustomer = nameCustomer;
            SurnameCustomer = surnameCustomer;
            RentalItems = rentalItems;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            PaymentMethod = paymentMethod;
            UserNameCustomer = userNameCustomer;
            RentalItems = rentalItems;
        }

        public int Id { get; set; }

        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        public string DeliveryAddress { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have at least 3 characters")]
        public string NameCustomer { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname must have at least 3 characters")]
        public string SurnameCustomer { get; set; }

        public IList<RentalItemDTO> RentalItems { get; set; }

        public DateTime RentalDateFrom { get; set; }

        public DateTime RentalDateTo { get; set; }


        [Required]
        public PaymentMethodType PaymentMethod { get; set; }

        [EmailAddress]
        [Required]
        public string UserNameCustomer { get; set; }

        [Precision(10, 2)]
        public decimal CostofRental { get; set; }

        public DateTime RentalDate { get; set; }

        public override bool Equals(object? obj) {
            return obj is RentalForDetailDTO dTO &&
                   Id == dTO.Id &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   NameCustomer == dTO.NameCustomer &&
                   SurnameCustomer == dTO.SurnameCustomer &&

                   //check that both collections are Equal
                   RentalItems.SequenceEqual(dTO.RentalItems) &&
                   RentalDateFrom == dTO.RentalDateFrom &&
                   RentalDateTo == dTO.RentalDateTo &&
                   PaymentMethod == dTO.PaymentMethod &&
                   UserNameCustomer == dTO.UserNameCustomer &&
                   CostofRental == dTO.CostofRental &&
                   RentalDate == dTO.RentalDate;
        }
    }
}

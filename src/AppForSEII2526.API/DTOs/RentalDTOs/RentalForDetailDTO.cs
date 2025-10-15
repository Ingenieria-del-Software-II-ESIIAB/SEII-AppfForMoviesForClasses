using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.RentalDTOs {
    public class RentalForDetailDTO:RentalForCreateDTO {
        public RentalForDetailDTO(int id, decimal costofRental, DateTime rentalDate,string deliveryAddress, 
            string nameCustomer, string surnameCustomer, 
            DateTime rentalDateFrom, DateTime rentalDateTo, PaymentMethodType paymentMethod, 
            string userNameCustomer, IList<RentalItemDTO> rentalItems)
                :base(deliveryAddress, nameCustomer, surnameCustomer, rentalDateFrom, rentalDateTo, paymentMethod, userNameCustomer, rentalItems) {
            Id = id;
            CostofRental = costofRental;
            RentalDate = rentalDate;
        }

        public int Id { get; set; }

        [Precision(10, 2)]
        public decimal CostofRental { get; set; }

        public DateTime RentalDate { get; set; }


    }
}

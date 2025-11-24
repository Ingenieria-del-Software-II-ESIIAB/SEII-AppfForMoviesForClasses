using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web {
    public class RentalStateContainer {

        //we create an instance of Rental when an instance of RentalStateContainer is created
        public RentalForCreateDTO Rental { get; private set; } = new RentalForCreateDTO() {
            RentalItems = new List<RentalItemDTO>()
        };

        //we compute the TotalPrice of the movies we have selected for renting them
        public decimal TotalPrice {
            get {
                int numberOfDays = (Rental.RentalDateTo - Rental.RentalDateFrom).Days;
                return Convert.ToDecimal(Rental.RentalItems.Sum(ri => ri.PriceForRenting * numberOfDays));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        
        public void AddMovieToRental(MovieForRentalDTO movie) {
            //before adding a movie we checked whether it has been already added
            if (!Rental.RentalItems.Any(ri => ri.MovieID == movie.Id))
                //we add it if it is not in the list
                Rental.RentalItems.Add(new RentalItemDTO() {
                    MovieID = movie.Id,
                    Genre = movie.Genre,
                    Title = movie.Title,
                    PriceForRenting = movie.PriceForRenting,
                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemoveRentalItemToRent(RentalItemDTO item) {
            Rental.RentalItems.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearRentingCart() {
            Rental.RentalItems.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void RentalProcessed() {
            //we have finished the rental process so we create a new object without data
            Rental = new RentalForCreateDTO() {
                RentalItems = new List<RentalItemDTO>()
            };
        }
    }
}

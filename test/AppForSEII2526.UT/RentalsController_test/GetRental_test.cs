using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UT.RentalsController_test {
    public class GetRentals_test : AppForMovies4SqliteUT {
        public GetRentals_test() {

            var genres = new List<Genre>() {
                new Genre("Sci - Fi"),
                new Genre("Drama"),
            };

            var movies = new List<Movie>(){
                new Movie("The lord of the rings", genres[0],new DateTime(2011, 10, 20),10.0m,10, 5,1),
                new Movie("The man in the high castle", genres[1],new DateTime(2015, 01, 01),10.0m,0, 4.0m,15),
            };

            _context.AddRange(movies);
            _context.SaveChanges();

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es", "Avda. España s/n, Albacete 02071");

            var rental = new Rental("Avda. España s/n, Albacete 02071", "Elena", "Navarro Martínez", user,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), PaymentMethodType.CreditCard, new List<RentalItem>());

            rental.RentalItems.Add(new RentalItem(movies[0].Id,rental, 1.0m, "My favourite movie"));

            decimal numDays = (decimal)(rental.RentalDateTo - rental.RentalDateFrom).TotalDays;
            rental.CostofRental = rental.RentalItems.Sum(ri => ri.Price * numDays);

            _context.Users.Add(user);
            _context.Add(rental);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test() {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.GetRental(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetRental_Found_test() {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;
            var controller = new RentalsController(_context, logger);


            var expectedRental = new RentalForDetailDTO(1, 3.0m, DateTime.Now, "Avda. España s/n, Albacete 02071",
                "Elena", "Navarro Martínez", DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                PaymentMethodType.CreditCard, "elena@uclm.es", new List<RentalItemDTO>());


            expectedRental.RentalItems.Add(new RentalItemDTO(1, "The lord of the rings", "Sci - Fi", 5.0m, "My favourite movie"));

            // Act 

            //we call the System Under Test (SUT)
            var result = await controller.GetRental(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);

            //we check that the type returned is equal than the expected
            var rentalDTOActual = Assert.IsType<RentalForDetailDTO>(okResult.Value);


            //we check that the expected and actual data are the same
            Assert.Equal(expectedRental, rentalDTOActual);

        }
    }
}
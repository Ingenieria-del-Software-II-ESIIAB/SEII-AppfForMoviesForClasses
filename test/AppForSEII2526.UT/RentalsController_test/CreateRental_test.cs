using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.RentalsController_test {
    public class CreateRental_test:AppForMovies4SqliteUT {

        public CreateRental_test() {

            var genres = new List<Genre>() {
                new Genre("Sci - Fi"),
                new Genre("Drama"),
            };

            var movies = new List<Movie>(){
                new Movie("The lord of the rings", genres[0],new DateTime(2011, 10, 20),10.0m,10, 5,1),
                new Movie("The man in the high castle", genres[1],new DateTime(2015, 01, 01),10.0m,0, 4.0m,15),
            };

            //add movies and the genres they are related
            _context.AddRange(movies);

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es", "Avda. España s/n, Albacete 02071");
            _context.Add(user);

            _context.SaveChanges();

            var rental = new Rental("Avda. España s/n, Albacete 02071", "Elena", "Navarro Martínez", user,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), 
                PaymentMethodType.CreditCard, new List<RentalItem>());

            rental.RentalItems.Add(new RentalItem(movies[0].Id, rental, 1.0m, "My favourite movie"));

            decimal numDays = (decimal)(rental.RentalDateTo - rental.RentalDateFrom).TotalDays;
            rental.CostofRental = rental.RentalItems.Sum(ri => ri.Price * numDays);

            _context.Add(rental);

            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase() {
            RentalForCreateDTO rentalNoITem = new RentalForCreateDTO("Avda. España s/n, Albacete", "Elena", "Navarro",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), 
                PaymentMethodType.CreditCard, "elena@uclm.es",
                new List<RentalItemDTO>());


            IList<RentalItemDTO> rentalItems = new List<RentalItemDTO>() { new RentalItemDTO(2, "The man in the high castle", "Drama", 4.0m, "My favourite movie") };

            RentalForCreateDTO rentalFromBeforeToday = new RentalForCreateDTO("Avda. España s/n, Albacete", "Elena", "Navarro",
                DateTime.Today, DateTime.Today.AddDays(5),
                PaymentMethodType.CreditCard, "elena@uclm.es",
                rentalItems);

            RentalForCreateDTO rentalToBeforeFrom = new RentalForCreateDTO("Avda. España s/n, Albacete", "Elena", "Navarro",
                DateTime.Today.AddDays(5), DateTime.Today.AddDays(2),
                PaymentMethodType.CreditCard, "elena@uclm.es",
                rentalItems);

            //in the constructor elena@uclm.es was registered but not victor.lopez@uclm.es
            RentalForCreateDTO applicationUserNotRegistered = new RentalForCreateDTO("Avda. España s/n, Albacete", "Elena", "Navarro",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                PaymentMethodType.CreditCard, "victor.lopez@uclm.es",
                rentalItems);

            RentalForCreateDTO rentalMovieNotAvailable = new RentalForCreateDTO("Avda. España s/n, Albacete", "Elena", "Navarro",
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                PaymentMethodType.CreditCard, "victor.lopez@uclm.es",
                new List<RentalItemDTO>() { new RentalItemDTO(1, "The lord of the rings", "Sci - Fi",  1.0m, "I like it") });




            var allTests = new List<object[]>
            {             //input for createpurchase - Error expected
                new object[] { rentalNoITem, "Error! You must include at least one movie to be rented",  },
                new object[] { rentalFromBeforeToday, "Error! Your rental date must start later than today", },
                new object[] { rentalToBeforeFrom, "Error! Your rental must end later than it starts", },
                new object[] { applicationUserNotRegistered, "Error! UserName is not registered", },
                new object[] { rentalMovieNotAvailable, "Error! Movie titled 'The lord of the rings' is not available for being rented from", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreateRental_Error_test(RentalForCreateDTO rentalDTO, string errorExpected) {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.CreateRental(rentalDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }



    }
}

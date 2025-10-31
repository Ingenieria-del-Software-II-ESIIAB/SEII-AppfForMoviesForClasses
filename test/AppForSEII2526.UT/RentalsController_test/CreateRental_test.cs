using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTOs;


namespace AppForSEII2526.UT.RentalsController_test {
    public class CreateRental_test:AppForMovies4SqliteUT {

        //we define the data for the test to facilitate its reuse
        private const string _deliveryAddress = "Avda. España s/n, Albacete 02071";
        private const string _surnameCustomer = "Navarro Martínez";
        private const string _nameCustomer = "Elena";
        private const string _userNameCustomer = "elena@uclm.es";
        private const string _movie1Title = "The lord of the rings";
        private const string _movie1Genre = "Sci - Fi";
        private const string _movie2Title = "The man in the high castle";
        private const string _movie2Genre = "Drama";


        public CreateRental_test() {

            var genres = new List<Genre>() {
                new Genre(_movie1Genre),
                new Genre(_movie2Genre),
            };

            var movies = new List<Movie>(){
                //we state that there is only one movie available for renting
                new Movie(_movie1Title, genres[0],new DateTime(2011, 10, 20),10.0m,10, 5,1),

                
                new Movie(_movie2Title, genres[1],new DateTime(2015, 01, 01),10.0m,0, 4.0m,15),
            };

            //add movies and the genres they are related
            _context.AddRange(movies);

            ApplicationUser user = new ApplicationUser("1", _nameCustomer, _surnameCustomer, _userNameCustomer, _deliveryAddress);
            _context.Add(user);

            _context.SaveChanges();

            var rental = new Rental(_deliveryAddress, _nameCustomer, _surnameCustomer, user,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), 
                PaymentMethodType.CreditCard, new List<RentalItem>());

            rental.RentalItems.Add(new RentalItem(movies[0].Id, rental, 1.0m, "My favourite movie"));

            decimal numDays = (decimal)(rental.RentalDateTo - rental.RentalDateFrom).TotalDays;
            rental.CostofRental = rental.RentalItems.Sum(ri => ri.Price * numDays);

            _context.Add(rental);

            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase() {
            RentalForCreateDTO rentalNoITem = new RentalForCreateDTO(_deliveryAddress, _nameCustomer, _surnameCustomer,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), 
                PaymentMethodType.CreditCard, _userNameCustomer,
                new List<RentalItemDTO>());


            IList<RentalItemDTO> rentalItems = new List<RentalItemDTO>() { new RentalItemDTO(2, _movie2Title, _movie2Genre, 4.0m, "My favourite movie") };

            RentalForCreateDTO rentalFromBeforeToday = new RentalForCreateDTO(_deliveryAddress, _nameCustomer, _surnameCustomer,
                DateTime.Today, DateTime.Today.AddDays(5),
                PaymentMethodType.CreditCard, _userNameCustomer,
                rentalItems);

            RentalForCreateDTO rentalToBeforeFrom = new RentalForCreateDTO(_deliveryAddress, _nameCustomer, _surnameCustomer,
                DateTime.Today.AddDays(5), DateTime.Today.AddDays(2),
                PaymentMethodType.CreditCard, _userNameCustomer,
                rentalItems);

            //in the constructor elena@uclm.es was registered but not victor.lopez@uclm.es
            RentalForCreateDTO applicationUserNotRegistered = new RentalForCreateDTO(_deliveryAddress, _nameCustomer, _surnameCustomer,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                PaymentMethodType.CreditCard, "victor.lopez@uclm.es",
                rentalItems);

            RentalForCreateDTO rentalMovieNotAvailable = new RentalForCreateDTO(_deliveryAddress, _nameCustomer, _surnameCustomer,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                PaymentMethodType.CreditCard, "victor.lopez@uclm.es",
                new List<RentalItemDTO>() { new RentalItemDTO(1, _movie1Title, _movie1Genre,  1.0m, "I like it") });


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

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateRental_Success_test() {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            //we use always relative dates to avoid errors if the test is run some years later
            DateTime from = DateTime.Today.AddDays(2);
            DateTime to = DateTime.Today.AddDays(5);

            RentalForCreateDTO rentalDTO = new RentalForCreateDTO(_deliveryAddress, _nameCustomer, _surnameCustomer,
                from, to,
                PaymentMethodType.CreditCard, _userNameCustomer,
                new List<RentalItemDTO>());
            rentalDTO.RentalItems.Add( new RentalItemDTO(2, _movie2Title, _movie2Genre, 4.0m, "My favourite movie") );


                                //the id is 2 because there is another rental in the database
            RentalForDetailDTO expectedrentalDetailDTO = new RentalForDetailDTO(2, 
                12.0m, DateTime.Now, _deliveryAddress,
                _nameCustomer, _surnameCustomer, from, to,
                PaymentMethodType.CreditCard, _userNameCustomer, new List<RentalItemDTO>());
            expectedrentalDetailDTO.RentalItems.Add(new RentalItemDTO(2, _movie2Title, _movie2Genre, 4.0m, "My favourite movie"));

            // Act
            var result = await controller.CreateRental(rentalDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualRentalDetailDTO = Assert.IsType<RentalForDetailDTO>(createdResult.Value);

            Assert.Equal(expectedrentalDetailDTO, actualRentalDetailDTO);

        }

    }
}

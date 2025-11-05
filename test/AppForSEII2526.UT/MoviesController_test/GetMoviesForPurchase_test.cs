using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.MovieDTOs;

namespace AppForSEII2526.UT.MoviesController_test {
    public class GetMoviesForPurchase_test : AppForMovies4SqliteUT {
        public GetMoviesForPurchase_test() {

            var genres = new List<Genre>() {
                    new Genre("Sci - Fi"),
                    new Genre("Drama"),
            };

            var movies = new List<Movie>(){
                    new Movie("The lord of the rings", genres[0],new DateTime(2011, 10, 20),10.0m, 5,1.0m,1),
                    new Movie("The mechanic orange", genres[1],new DateTime(1988, 02, 23),15.0m, 10,2.0m,2),

             //this movie has quantityforpurchase=0 and quantityforrenting=0 so it shouldn't be returned when 
                    new Movie("The flying castle", genres[1],new DateTime(2007, 04, 04),20.0m, 0,3.0m,10),
            };
            _context.AddRange(genres);
            _context.AddRange(movies);
            _context.SaveChanges();

        }

        //[Fact]
        //[Trait("LevelTesting", "Unit Testing")]
        //public async Task GetMoviesForPurchase_NULL_TITLE_GENRE() {
        //    // Arrange
        //    var expectedMovies = new List<MovieForPurchaseDTO>() {
        //        new MovieForPurchaseDTO(1,"The lord of the rings","Sci - Fi",new DateTime(2011, 10, 20),10.0m),
        //        new MovieForPurchaseDTO(2,"The mechanic orange","Drama" , new DateTime(1988, 02, 23),15.0m),
        //    };

        //    var controller = new MoviesController(_context, null);

        //    // Act
        //    var result = await controller.GetMoviesForPurchase(null, null);

        //    //Assert
        //    //we check that the response type is OK 
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    //and obtain the list of movies
        //    var movieDTOsActual = Assert.IsType<List<MovieForPurchaseDTO>>(okResult.Value);
        //    Assert.Equal(expectedMovies, movieDTOsActual);

        //}

        public static IEnumerable<object[]> TestCasesFor_GetMoviesForPurchase_OK() {

            var movieDTOs = new List<MovieForPurchaseDTO>() {
                new MovieForPurchaseDTO(1,"The lord of the rings","Sci - Fi",new DateTime(2011, 10, 20),10.0m),
                new MovieForPurchaseDTO(2,"The mechanic orange","Drama" , new DateTime(1988, 02, 23),15.0m),

            };

            var movieDTOsTC1 = new List<MovieForPurchaseDTO>() { movieDTOs[0], movieDTOs[1] };
                    //the GetMoviesForPurchase method returns the movies ordered by title



            var movieDTOsTC2 = new List<MovieForPurchaseDTO>() { movieDTOs[0] };
            var movieDTOsTC3 = new List<MovieForPurchaseDTO>() { movieDTOs[1] };



            var allTests = new List<object[]>
            {             //filters to apply - expected movies
                                          //by default datefrom=today +1, dateto=today+2, thus movieDTOs[0] cannot be returned
                new object[] { null, null, movieDTOsTC1,  },
                new object[] { "xxx", null, movieDTOsTC2, },
                new object[] { null, "Drama", movieDTOsTC3, },

            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetMoviesForPurchase_OK))]
        public async Task GetMoviesForPurchase_filter_test(string? movieTitle, string? movieGenre, List<MovieForPurchaseDTO> expectedMovies) {
            // Arrange

            var controller = new MoviesController(_context, null);

            // Act
            var result = await controller.GetMoviesForPurchase(movieTitle, movieGenre);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var movieDTOsActual = Assert.IsType<List<MovieForPurchaseDTO>>(okResult.Value);
            Assert.Equal(expectedMovies, movieDTOsActual);

        }
    }
}

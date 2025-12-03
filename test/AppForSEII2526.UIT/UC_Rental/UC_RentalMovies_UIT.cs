using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Rental {
    public class UC_RentalMovies_UIT : UC_UIT {
        private SelectMoviesForRental_PO selectMoviesForRental_PO;
        private const int movieId1 = 1;
        private const string movieTitle1 = "The last of us";
        private const string movieGenre1 = "Sci - Fi";
        private const string moviePriceForRenting1 = "1";
        private const string movieReleaseDate1 = "15/03/2023";
        private const string movieTitle2 = "The man in the high castle";
        private const string movieGenre2 = "Drama";
        private const string moviePriceForRenting2 = "3";
        private const string movieReleaseDate2 = "15/01/2015";



        public UC_RentalMovies_UIT(ITestOutputHelper output) : base(output) {

            selectMoviesForRental_PO = new SelectMoviesForRental_PO(_driver, _output);
        }

        private void Precondition_perform_login() {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        private void InitialStepsForRentalMovies() { 
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectMoviesForRental_PO.WaitForBeingVisible(By.Id("CreateRental"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateRental")).Click();
        }

        [Theory]
        [InlineData(movieTitle1, movieGenre1, movieReleaseDate1, moviePriceForRenting1, "Last of", "")]
        [InlineData(movieTitle2, movieGenre2, movieReleaseDate2, moviePriceForRenting2, "", "Drama")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_UC2_4_5_filtering(string movieTitle, string movieGenre, string movieReleaseDate, string moviePriceForRenting,
            string filterTitle, string filterGenre)
        {
            //Arrange
            InitialStepsForRentalMovies();
            var expectedMovies = new List<string[]> { new string[] { movieTitle, movieGenre, movieReleaseDate, moviePriceForRenting}, };

            //Act
            selectMoviesForRental_PO.SearchMovies(filterTitle, filterGenre, "", "");

            //Assert

            Assert.True(selectMoviesForRental_PO.CheckListOfMovies(expectedMovies));

        }
        [Fact(Skip = "first run dbo.Movies.data.UpdateQuantityAvailable.sql, after running the test case run dbo.Movies.data.UpdateQuantityAvailableto100")]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC2_AF1_UC2_6_filtering() {
            //Arrange
            InitialStepsForRentalMovies();
            var expectedMovies = new List<string[]> { new string[] { movieTitle2, movieGenre2, movieReleaseDate2, moviePriceForRenting2 }, };

            string from = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy");
            string to = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");
            //Act
            selectMoviesForRental_PO.SearchMovies("", "", from, to);

            //Assert

            Assert.True(selectMoviesForRental_PO.CheckListOfMovies(expectedMovies));

        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Rental {
    public class UC_RentalMovies_UIT : UC_UIT {
        private SelectMoviesForRental_PO selectMoviesForRental_PO;
        private const int movieId1 = 1;
        private const string movieTitle1 = "The man in the high castle";
        private const string movieGenre1 = "Sci - Fi";
        private const string moviePriceForRenting1 = "4";
        private const string movieReleaseDate1 = "1/1/2015";




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
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_UC2_4_5_6_filtering()
        {
            //Arrange
            InitialStepsForRentalMovies();
            var expectedMovies = new List<string[]> { new string[] { movieTitle1, movieGenre1, movieReleaseDate1, moviePriceForRenting1 },};

            //Act
            selectMoviesForRental_PO.SearchMovies("Last of", "", "", "");

            //Assert

            Assert.True(selectMoviesForRental_PO.CheckListOfMovies(expectedMovies));

        }



    }
}

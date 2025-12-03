using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Rental {
    public class SelectMoviesForRental_PO : PageObject {
        By inputTitle = By.Id("inputTitle");
        By inputGenre = By.Id("selectGenre");
        By buttonSearchMovies = By.Id("searchMovies");
        By inputFrom = By.Id("fromDate");
        By inputTo = By.Id("toDate");
        By tableOfMoviesBy = By.Id("TableOfMovies");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonRentMovies=By.Id("rentMovieButton");

        public SelectMoviesForRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }
        public void SearchMovies(string title, string genre, string from, string to) {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputTitle);
            _driver.FindElement(inputTitle).SendKeys(title);
            if (genre == "") genre = "All";
            SelectElement selectElement = new SelectElement(_driver.FindElement(inputGenre));
            selectElement.SelectByText(genre);

            if (from!="") 
                _driver.FindElement(inputFrom).SendKeys(from);

            if (to != "")
                _driver.FindElement(inputTo).SendKeys(to);

            _driver.FindElement(buttonSearchMovies).Click();

        
        }


        public bool CheckListOfMovies(List<string[]> expectedMovies) {

            return CheckBodyTable(expectedMovies, tableOfMoviesBy);
        }

        public bool CheckMessageError(string errorMessage) {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void AddMovieToRentingCart(string movieTitle) {
            WaitForBeingClickable(By.Id("movieToRent_" + movieTitle));

            _driver.FindElement(By.Id("movieToRent_"+ movieTitle)).Click();
        }

        public void RemoveMovieFromRentingCart(string movieTitle) {
            WaitForBeingClickable(By.Id("removeMovie_" + movieTitle));
            _driver.FindElement(By.Id("removeMovie_" + movieTitle)).Click();
        }

        public bool RentingNotAvailable() {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonRentMovies).Displayed==false;
        }

    }
}

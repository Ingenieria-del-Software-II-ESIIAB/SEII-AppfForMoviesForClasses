using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Rental {
    public class SelectMoviesForRental_PO : PageObject {
        By inputTitle = By.Id("movieTitle");
        By inputGenre = By.Id("selectGenre");
        By buttonSearchMovies = By.Id("searchMovies");
        public SelectMoviesForRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }
        public void SearchMovies(string title) {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputTitle);
            _driver.FindElement(inputTitle).SendKeys(title);
            _driver.FindElement(buttonSearchMovies).Click();

        
        }
    }
}

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.Shared {
    public class PageObject {

        protected IWebDriver _driver;
        //this may be used whenever some result should be printed in Explorador de Pruebas
        protected readonly ITestOutputHelper _output;

        private By _modalTitle = By.ClassName("modal-title");
        private By _modalBody = By.ClassName("modal-body");
        private By _okModalDialog = By.Id("Button_DialogOK");


        protected PageObject(IWebDriver driver, ITestOutputHelper output) {
            _driver = driver;
            this._output = output;
        }


        public void InputDateInDatePicker(By datepicker, DateTime date) {
            //first we select the datepicker
            IWebElement webElement = _driver.FindElement(datepicker);

            var action = new Actions(_driver);
            webElement.Clear();
            webElement.Click();
            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Left).Perform();
            action.SendKeys(date.ToString("dd")).Perform();

            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Right).Perform();
            action.SendKeys(date.ToString("MM")).Perform();

            action.KeyDown(Keys.Right).Perform();
            action.KeyDown(Keys.Right).Perform();
            action.SendKeys(date.ToString("yyyy")).Perform();

        }


        public bool CheckBodyTable(List<string[]> expectedRows, By IdTable) {
            string expectedRow, actualRow;
            int i, j;
            bool result = true;
            WaitForBeingVisible(IdTable);

            IList<IWebElement> actualrows = _driver
                .FindElement(IdTable)
                .FindElement(By.TagName("tbody"))
                //.FindElements(By.XPath(".//tr"))
                .FindElements(By.TagName("tr"))//we obtain just the rows of the body of the table
                .ToList();

            if (actualrows.Count != expectedRows.Count) {
                _output.WriteLine($"Error: \n Expected number of rows:{expectedRows.Count} \n Actual number of rows:{actualrows.Count}");
                return false;
            }

            for (i = 0; i < expectedRows.Count; i++) {
                expectedRow = expectedRows[i][0];
                for (j = 1; j < expectedRows[i].Count(); j++)
                    expectedRow = expectedRow + " " + expectedRows[i][j];
                actualRow = actualrows
                    .Select(m => m.Text) //we return the text of the row
                    .ToList()[i];

                if (!actualRow.StartsWith(expectedRow)) {
                    _output.WriteLine($"Error: \n \t expected row:{expectedRow} \n \t actual row:{actualRow}");
                    result = false;

                }
            }
            return result;

        }

        public bool CheckModalBodyText(string expectedBody, By modal) {
            //waiting for the message error to be shown
            WaitForBeingVisible(modal);
            var actualBody = _driver.FindElement(_modalBody).Text;
            return actualBody.Contains(expectedBody);
        }

        public bool CheckModalTitleText(string expectedTitle, By modal) {
            //waiting for the message error to be shown
            WaitForBeingVisible(modal);
            var actualTitle = _driver.FindElement(_modalTitle).Text;
            return actualTitle.Contains(expectedTitle);
        }

        public void PressOkModalDialog() {
            //waiting for the message error to be shown
            WaitForBeingVisible(_okModalDialog);
            _driver.FindElement(_okModalDialog).Click();
        }



        public void WaitForBeingClickable(By IdElement) {
            //used whenever the webelement needs a delay for being clickable
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            wait.Until(ExpectedConditions.ElementToBeClickable(IdElement));

        }

        public void WaitForBeingVisible(By IdElement) {
            //used whenever the webelement needs a delay for being clickable
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            wait.Until(ExpectedConditions.ElementIsVisible(IdElement));

        }

        public void WaitForBeingVisibleIgnoringExeptionTypes(By IdElement) {
            //used whenever the webelement needs a delay for being clickable
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 10, 0));


            wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
                typeof(WebDriverTimeoutException),
                typeof(UnhandledAlertException),
                typeof(ElementClickInterceptedException));
            bool notFoundButton = true;
            while (notFoundButton) {
                try {
                    wait.Until(ExpectedConditions.ElementIsVisible(IdElement));
                    notFoundButton = false;
                }
                catch (ElementClickInterceptedException ex) {
                    _output.WriteLine(ex.Message);
                }
            }
        }


        public void WaitForTextToBePresentInElement(By IdElement, string expectedText) {
            //used whenever the webelement needs a delay for being clickable
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            IWebElement element = _driver.FindElement(IdElement);
            wait.Until(ExpectedConditions.TextToBePresentInElement(element, expectedText));

        }


        //it wait for "seconds" till all the webelements of the page are loaded
        public void ImplicitWait(int seconds) =>
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
    }
}


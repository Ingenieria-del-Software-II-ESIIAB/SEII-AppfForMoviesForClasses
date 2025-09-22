using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;


namespace AppForMovies.UIT.Shared {
    public class UC_UIT : IDisposable {

        private bool _pipeline = false;

        //establish which browser you would like to use
        //private string _browser = "Chrome";
        //private string _browser = "Firefox";
        private string _browser = "Edge";

        protected IWebDriver _driver;
        protected readonly ITestOutputHelper _output;


        public string _URI {
            get {
                //set url of your web page 
                return "https://localhost:7083/";

            }
        }

        public UC_UIT(ITestOutputHelper output) {

            //it initializes where the errors will be shown
            _output = output;

            switch (_browser) {
                case "Firefox":
                    SetUp_FireFox4UIT();
                    break;
                case "Edge":
                    SetUp_EdgeFor4UIT();
                    break;
                default:
                    //by default Chrome will be used
                    SetUp_Chrome4UIT();
                    break;
            }
            //Added to make _Driver wait when an element is not found.
            //It will wait for a maximum of 50 seconds.

            //maximize the window browser
            _driver.Manage().Window.Maximize();
        }


        protected void Initial_step_opening_the_web_page() {
            _driver.Navigate()
                .GoToUrl(_URI);
        }

        protected void Perform_login(string email, string password) {
            _driver.Navigate()
                    .GoToUrl(_URI + "Account/Login");
            // _driver.FindElement(By.Id("Input_Email"))
            //     .SendKeys("elena.navarro@uclm.es");
            _driver.FindElement(By.Name("Input.Email"))
                .SendKeys(email);

            _driver.FindElement(By.Name("Input.Password"))
                .SendKeys(password);

            _driver.FindElement(By.XPath("/html/body/div[1]/main/article/div/div[1]/section/form/div[4]/button"))
                .Click();
        }


        protected void SetUp_Chrome4UIT() {
            var optionsc = new ChromeOptions {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            //For pipelines use this option for hiding the browser
            if (_pipeline) optionsc.AddArgument("--headless");

            _driver = new ChromeDriver(optionsc);

        }

        protected void SetUp_FireFox4UIT() {
            var optionsff = new FirefoxOptions {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            //For pipelines use this option for hiding the browser
            if (_pipeline) optionsff.AddArgument("--headless");

            _driver = new FirefoxDriver(optionsff);

        }

        protected void SetUp_EdgeFor4UIT() {
            //var edgeDriverService = Microsoft.Edge.SeleniumTools.EdgeDriverService.CreateChromiumService();
            //var edgeOptions = new Microsoft.Edge.SeleniumTools.EdgeOptions();
            //edgeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            //edgeOptions.UseChromium = true;
            //if (_pipeline) edgeOptions.AddArguments("--headless");

            //_driver = new Microsoft.Edge.SeleniumTools.EdgeDriver(edgeDriverService, edgeOptions);

            var optionsEdge = new EdgeOptions {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            //For pipelines use this option for hiding the browser
            if (_pipeline) optionsEdge.AddArgument("--headless");

            _driver = new EdgeDriver(optionsEdge);

        }


        public void Dispose() {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
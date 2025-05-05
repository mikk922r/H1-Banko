using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Banko
{
    public class Scraper
    {
        public List<Plate> GetPlates(string[] plateIds)
        {
            List<Plate> plates = new List<Plate>();

            IWebDriver driver = GetWebDriver();

            driver.Navigate().GoToUrl("https://mercantech.github.io/Banko/");

            foreach (string plateId in plateIds)
            {
                Plate plate = new Plate()
                {
                    Id = plateId,
                    Numbers = new List<List<int>>()
                };

                IWebElement input = driver.FindElement(By.Id("tekstboks"));

                input.Clear();

                input.SendKeys(plateId);

                IWebElement button = driver.FindElement(By.Id("knap"));

                button.Click();

                IReadOnlyCollection<IWebElement> plateElements = driver.FindElements(By.TagName("tr"));

                foreach (IWebElement plateElement in plateElements)
                {
                    IReadOnlyCollection<IWebElement> plateNumbers = plateElement.FindElements(By.TagName("td"));

                    List<int> numbers = plateNumbers.Where(x => !string.IsNullOrEmpty(x.Text)).Select(x => Convert.ToInt32(x.Text)).ToList();

                    plate.Numbers.Add(numbers);
                }

                plates.Add(plate);
            }

            driver.Quit();

            return plates;
        }

        private IWebDriver GetWebDriver()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();

            service.EnableVerboseLogging = false;
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();

            options.AddArgument("--headless");

            return new ChromeDriver(service, options);
        }
    }
}

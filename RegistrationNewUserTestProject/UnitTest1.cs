using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Linq;


namespace RegistrationNewUserTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver driver;
        private WebDriverWait wait;


        [TestInitialize]
        public void Init()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        }


        private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnoprstuvwxyz";
        private const string numbers = "0123456789";
        private const string alphanumbers = alphabet + numbers;
        private static Random random = new Random();

        private static string RandomString(string str, int length)
        {
            return new string(Enumerable.Repeat(str, length).Select(s => s[random.Next(s.Length)]).ToArray());

        }

        private static string RandomAlphabetString(int length)
        {
            return RandomString(alphabet, length);
        }

        private static string RandomNumbersString(int length)
        {
            return RandomString(numbers, length);
        }

        private static string RandomAlphanumbersString(int length)
        {
            return RandomString(alphanumbers, length);
        }


        private void SelectElementByText(By by, string element)
        {
            SelectElement select = new SelectElement(driver.FindElement(by));
            select.SelectByText(element);
        }


        private void SetCheckBoxState(IWebElement checkBox, bool needState)
        {
            string currentState = checkBox.GetAttribute("checked");
            if (Convert.ToBoolean(currentState) != needState)
            {
                checkBox.Click();
            }
        }


        private void Login(string email, string password)
        {
            driver.FindElement(By.CssSelector("input[name='email']")).SendKeys(email);
            driver.FindElement(By.CssSelector("input[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("button[name='login']")).Click(); // [Login]
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#box-account")));
        }


        private void Logout()
        {
            driver.FindElement(By.CssSelector("a[href='http://litecart/en/logout']")).Click(); // [Logout]
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#box-account-login")));
        }


        [TestMethod]
        public void RegistrationNewUserTest()
        {
            driver.Url = "http://litecart/";

            string newCustomerLinkSelector = "#box-account-login a[href*='create_account']";
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(newCustomerLinkSelector)));
            driver.FindElement(By.CssSelector(newCustomerLinkSelector)).Click(); // 'New customers click here'

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#create-account form[name='customer_form']")));

            string taxID = RandomAlphanumbersString(7); // Tax ID
            string company = RandomAlphabetString(8); // Company
            string firstName = RandomAlphabetString(10); // First Name
            string lastName = RandomAlphabetString(10); // Last Name
            string address1 = RandomAlphanumbersString(12); // Address 1
            string address2 = RandomAlphanumbersString(12); // Address 2
            string postcode = "T7S 1R3"; // Postcode
            string city = "DefaultCity"; // City
            string country = "Canada"; // Country
            string zone = "Ontario"; // Zone/State/Province
            string email = "a" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15) + "@gmail.com"; // Email
            string phone = "+7" + RandomNumbersString(10); // Phone
            string password = RandomAlphanumbersString(12); // Desired Password
            bool isSubscribe = false; // Newsletter Subscribe


            driver.FindElement(By.CssSelector("input[name='tax_id']")).SendKeys(taxID);
            driver.FindElement(By.CssSelector("input[name='company']")).SendKeys(company);
            driver.FindElement(By.CssSelector("input[name='firstname']")).SendKeys(firstName);
            driver.FindElement(By.CssSelector("input[name='lastname']")).SendKeys(lastName);
            driver.FindElement(By.CssSelector("input[name='address1']")).SendKeys(address1);
            driver.FindElement(By.CssSelector("input[name='address2']")).SendKeys(address2);
            driver.FindElement(By.CssSelector("input[name='postcode']")).SendKeys(postcode);
            driver.FindElement(By.CssSelector("input[name='city']")).SendKeys(city);

            SelectElementByText(By.CssSelector("select[name='country_code']"), country);

            wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.CssSelector("select[name='zone_code']"))));
            SelectElementByText(By.CssSelector("select[name='zone_code']"), zone);


            driver.FindElement(By.CssSelector("input[name='email']")).SendKeys(email);
            driver.FindElement(By.CssSelector("input[name='phone']")).SendKeys(phone);
            driver.FindElement(By.CssSelector("input[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("input[name='confirmed_password']")).SendKeys(password);

            IWebElement newsletter = driver.FindElement(By.CssSelector("input[name='newsletter']"));
            SetCheckBoxState(newsletter, isSubscribe);

            driver.FindElement(By.CssSelector("button[name='create_account']")).Click(); // [Create Account]

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#box-account")));

            Logout();

            Login(email, password);

            Logout();
        }


        [TestCleanup]
        public void Finish()
        {
            driver.Quit();
            //driver = null;
        }
    }
}

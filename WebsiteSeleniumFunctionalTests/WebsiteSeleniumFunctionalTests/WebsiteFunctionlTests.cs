using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace WebsiteSeleniumFunctionalTests
{
    /// <summary>
    ///  The functional tests to check whether the davies-group website performs expected behaviour.
    /// </summary>
    public class WebsiteSeleniumFunctionalTests
    {
        /// <summary>
        ///  Declare the selenium web driver to be used in the tests.
        /// </summary>
        private RemoteWebDriver _seleniumDriver;

        [SetUp]
        public void Setup()
        {
            _seleniumDriver = new ChromeDriver();
            _seleniumDriver.Manage().Window.Maximize();
            _seleniumDriver.Navigate().GoToUrl("https://davies-group.com/");
        }
        
        [TestCase("Twitter")]
        [TestCase("linkedIn")]
        public void VerifyClickingTheSocialMediaIconNavigatesToTheRespectivePage(string socialNetworkName)
        {
            // Act
            string correctSocialNetworkUrl = null;
            IJavaScriptExecutor executor = _seleniumDriver;
            var mainPageUrl = _seleniumDriver.FindElementByTagName("a").GetAttribute("href");
            var pageFooter = _seleniumDriver.FindElementByClassName("footer__socials");
            var socialNetworks = pageFooter.FindElements(By.TagName("a"));

            switch (socialNetworkName.ToLowerInvariant())
            {
                case "twitter":

                    correctSocialNetworkUrl = socialNetworks[0].GetAttribute("href");
                    var twitterIcon = _seleniumDriver.FindElementByCssSelector(
                        "body > div.page-wrapper > footer > div > div.footer__row > div.footer__share > ul > li:nth-child(1) > a");
                    executor.ExecuteScript("arguments[0].click();", twitterIcon);
                    break;

                case "linkedin":

                    correctSocialNetworkUrl = socialNetworks[1].GetAttribute("href");
                    var linkedIcon = _seleniumDriver.FindElementByCssSelector(
                        "body > div.page-wrapper > footer > div > div.footer__row > div.footer__share > ul > li:nth-child(2) > a");
                    executor.ExecuteScript("arguments[0].click();", linkedIcon);
                    break;
            }

            // Assert
            Assert.AreNotEqual(correctSocialNetworkUrl, mainPageUrl);
        }

        [Test]
        public void VerifyScrollingDownToFireInvestigationPageIsWorkingAsExpected()
        {
            //Act
            IJavaScriptExecutor jsExecutor = _seleniumDriver;
            var solutionButton = _seleniumDriver.FindElementByCssSelector("#menu-item-18257 > a");
            jsExecutor.ExecuteScript("arguments[0].click();", solutionButton);

            Thread.Sleep(20000);
            // Select to use the necessary cookies when using the website
            var useNecessaryCookies =
                _seleniumDriver.FindElementByXPath("//*[@id=\"CybotCookiebotDialogBodyLevelButtonLevelOptinDeclineAll\"]");
            jsExecutor.ExecuteScript("arguments[0].click();", useNecessaryCookies);

            // Scroll down to View All and load more case study as 'Fire Investigation' is not on the first page.
            var viewAllElement = _seleniumDriver.FindElementByCssSelector("body > div.page-wrapper > div > section.dg-cases-section.dg-cases-section--solutions > div > ul");
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", viewAllElement);
            Thread.Sleep(20000);
            var viewAllButton = _seleniumDriver.FindElementByCssSelector(
                "body > div.page-wrapper > div > section.dg-cases-section.dg-cases-section--solutions > div > div > a > p");
            jsExecutor.ExecuteScript("arguments[0].click();", viewAllButton);

            var loadMoreButton = _seleniumDriver.FindElementByCssSelector(
                "body > div.page-wrapper > div > section.case-archive > div.case-archive__body.bg--dark-blue > div > div.case-archive__footer.flex-container.align-center.align-middle > button");
            jsExecutor.ExecuteScript("arguments[0].click();", loadMoreButton);
            Thread.Sleep(30000);
            jsExecutor.ExecuteScript("arguments[0].click();", loadMoreButton);
            Thread.Sleep(30000);

            // Check the fire investigation text is in the body and scroll down and click the button
            var body = _seleniumDriver.FindElementByCssSelector(
                "body > div.page-wrapper > div > section.case-archive > div.case-archive__body.bg--dark-blue > div > div.case-archive__post-list.position-relative");
            jsExecutor.ExecuteScript("arguments[0].innerHTML.search(\"Fire Investigation\");", body);

            var fireInvestigationView = _seleniumDriver.FindElementByCssSelector(
                "body > div.page-wrapper > div > section.case-archive > div.case-archive__body.bg--dark-blue > div > div.case-archive__post-list.position-relative > ul:nth-child(3) > li:nth-child(1) > a > div.dg-cases-section__thumbnail > img");
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", fireInvestigationView);

            var fireInvestigation = _seleniumDriver.FindElementByCssSelector(
                "body > div.page-wrapper > div > section.case-archive > div.case-archive__body.bg--dark-blue > div > div.case-archive__post-list.position-relative > ul:nth-child(3) > li:nth-child(1) > a > div.dg-cases-section__thumbnail > img");
            jsExecutor.ExecuteScript("arguments[0].click();", fireInvestigation);

            // Take the screenshot and save in the file.
            Thread.Sleep(30000);
            ITakesScreenshot screenShot = _seleniumDriver;
            screenShot.GetScreenshot().SaveAsFile("C:\\Screenshots\\FireInvestigationCaseStudy.png");

            //Assert
            Assert.IsTrue(File.Exists("C:\\Screenshots\\FireInvestigationCaseStudy.png"));
        }

        [Test]
        public void CaptureStokeOfficeAddressFromTheMap()
        {
            // Act
            IJavaScriptExecutor jsExecutor = _seleniumDriver;
            var aboutMenu = _seleniumDriver.FindElementByCssSelector("#menu-item-18261 > a");
            jsExecutor.ExecuteScript("arguments[0].mouseover;", aboutMenu);

            var locationMenu = _seleniumDriver.FindElementByCssSelector("#menu-item-18265 > a");
            jsExecutor.ExecuteScript("arguments[0].click();", locationMenu);

            Thread.Sleep(30000);
            // Select to use the necessary cookies when using the website
            var useNecessaryCookies =
                _seleniumDriver.FindElementByXPath("//*[@id=\"CybotCookiebotDialogBodyLevelButtonLevelOptinDeclineAll\"]");
            jsExecutor.ExecuteScript("arguments[0].click();", useNecessaryCookies);

            var ukAndIrelandButton = _seleniumDriver.FindElementByCssSelector("body > div.page-wrapper > div > section.banner.banner--location.bg-cover.pre-lazyload > div > div > div.banner__group.banner__group--location > button.banner__group-button.banner__group-button--location.text-semibold.red");
            jsExecutor.ExecuteScript("arguments[0].click();", ukAndIrelandButton);
            
            var ukMapLocation = _seleniumDriver.FindElementByCssSelector("#svgPath0");
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", ukMapLocation);

            var stokeOnTrentOffice = _seleniumDriver.FindElementByCssSelector("#marker0_11");
            jsExecutor.ExecuteScript("arguments[0].click();", stokeOnTrentOffice);
            
            Thread.Sleep(60000);
            ITakesScreenshot screenShot = _seleniumDriver;
            screenShot.GetScreenshot().SaveAsFile("C:\\Screenshots\\StokeOfficeAddressFromMap.png");

            //Assert
            Assert.IsTrue(File.Exists("C:\\Screenshots\\StokeOfficeAddressFromMap.png"));
        }

        [TearDown]
        public void Teardown()
        {
            _seleniumDriver.Quit();
        }
    }
}
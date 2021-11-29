using System.Collections.Generic;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;


namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var html = GetHtml();
            var data = ParseHtmlUsingHtmlAgilityPack(html);
        }

        private static string GetHtml()
        {
            var options = new ChromeOptions
            {
                BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
            };

            options.AddArguments("headless");

            var chrome = new ChromeDriver(options);
            chrome.Navigate().GoToUrl("https://github.com/AhmedTarekHasan");

            return chrome.PageSource;
        }

        private static List<(string RepositoryName, string Description)> ParseHtmlUsingHtmlAgilityPack(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var repositories =
                htmlDoc
                    .DocumentNode
                    .SelectNodes("//div[@class='js-pinned-items-reorder-container']/ol/li/div/div");

            List<(string RepositoryName, string Description)> data = new();

            foreach (var repo in repositories)
            {
                var name = repo.SelectSingleNode("div/div/span/a").InnerText;
                var description = repo.SelectSingleNode("p").InnerText;
                data.Add((name, description));
            }

            return data;
        }
    }
}
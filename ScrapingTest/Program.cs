using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScrapingTest
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task<IHtmlDocument> _Scraping(string url)
        {
            var response = await client.GetAsync(url);
            var source = await response.Content.ReadAsStringAsync();
            var parser = new HtmlParser();
            return await parser.ParseDocumentAsync(source);
        }

        static IHtmlDocument Scraping(string url)
        {
            return _Scraping(url).Result;
        }

        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var urlstring = @"https://park.ajinomoto.co.jp/recipe/card/800020/";
            //var urlstring = @"https://www.shuwasystem.co.jp/news/n27546.html";
            Console.WriteLine("Scraping Start...");

            var doc = Scraping(urlstring);

            var priceElement = doc.GetElementById("recipeMaterialList");
            
            var listItems = priceElement.GetElementsByTagName("dl")
                .Select(item =>
                {
                    var ingredient = item.QuerySelector("dt").TextContent.Trim();
                    var num = item.QuerySelector("dd").TextContent.Trim();
                    return new { Ingredient = ingredient, Num = num };
            });

            listItems.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.Ingredient} : {item.Num}");
            });
            Console.ReadLine();
        }
    }
}

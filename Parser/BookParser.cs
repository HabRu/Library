using BusinessLayer.Services.LibraryParser.Model;
using BusinessLayer.Services.LibraryParser.ParserInterfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Parser
{
    public class BookParser : IParser<BookParserModel>
    {
        private readonly IConfiguration configuration;

        public BookParser(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public BookParserModel Parse(string href)
        {
            try
            {
                var web = new HtmlWeb();

                var htmlDoc = web.Load(href);

                var title = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='biblio_book_name biblio-book__title-block']/h1").FirstChild.InnerText.Trim();
                var author = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='biblio_book_author']/a").InnerText.Trim();
                var year = int.Parse(htmlDoc.DocumentNode.SelectNodes("//ul[@class='biblio_book_info_detailed_left']/li").FirstOrDefault(elem => elem.FirstChild.InnerText == "Дата написания:")?.LastChild.InnerText.Trim().Substring(0, 4));
                var language = "Русский";
                var genre = htmlDoc.DocumentNode.SelectNodes("//div[@class='biblio_book_info']/ul/li").FirstOrDefault(elem => elem.FirstChild.InnerText == "Жанр:").LastChild.InnerText.Trim().Replace('/', ',');
                var publisher = htmlDoc.DocumentNode.SelectNodes("//ul[@class='biblio_book_info_detailed_right']/li").FirstOrDefault(elem => elem.FirstChild.InnerText == "Правообладатель:")?.LastChild.InnerText.Trim() ?? "ЛитРес";
                var imagePath = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='biblio-book-cover-wrapper']").FirstChild.FirstChild.Attributes["src"].Value;
                var localDirectory = configuration.GetValue<string>("ImagePathParser");
                var dbImagePath = configuration.GetValue<string>("ImagePath") + $"{title.Replace(" ", "")}.jpg";
                var localFilename = localDirectory + $"{title.Replace(" ", "")}.jpg";
                if (!File.Exists(localFilename))
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(imagePath, localFilename);
                    }
                }
                Console.WriteLine($"{title}-{author}");
                var book = new BookParserModel { Title = title, Authtor = author, Genre = genre, Language = language, Publisher = publisher, Image = dbImagePath, Year = year };
                return book;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BookParserModel> ParseAsync(string href)
        {
            var book = await Task.Run(() => Parse(href));
            return book;
        }
    }
}

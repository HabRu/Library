using BusinessLayer.Services.LibraryParser.ParserInterfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ListBooksParse : IParser<IEnumerable<string>>
    {
        public IEnumerable<string> Parse(string href)
        {
            var html = href;

            var web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var bookLists = htmlDoc.DocumentNode.SelectNodes("//div/a[@class='img-a']").Select(t => t.Attributes["href"]?.Value ?? "");

            return bookLists;
        }

        public async Task<IEnumerable<string>> ParseAsync(string href)
        {
            var bookLists = await Task.Run(() => Parse(href));

            return bookLists;
        }
    }
}

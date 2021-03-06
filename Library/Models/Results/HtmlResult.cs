﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Models.Results
{
    public class HtmlResult : IActionResult
    {
        private readonly string htmlCode;
        public HtmlResult(string html)
        {
            htmlCode = html;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var fullHtmlCode = "<!DOCTYPE html><html><head>";
            fullHtmlCode += "<title>Подтвердите свой Email</title>";
            fullHtmlCode += "<meta charset=utf-8 />";
            fullHtmlCode += "</head> <body>";
            fullHtmlCode += htmlCode;
            fullHtmlCode += "</body></html>";
            await context.HttpContext.Response.WriteAsync(fullHtmlCode);
        }
    }
}

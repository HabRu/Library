using Library.Tag;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BookFilterModel
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public string Authtor { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public SortState SortOrder { get; set; }

        public BookFilterModel()
        {
            Page = 1;
            SortOrder = SortState.NameAsc;
            PageSize = 3;
        }

    }
}

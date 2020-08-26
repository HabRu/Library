using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class IndexViewModel
    {
        public List<Book> NewBooks { get; set; }
        public List<Book> TopBooks { get; set; }
    }
}

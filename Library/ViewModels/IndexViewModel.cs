using Library.Models;
using System.Collections.Generic;

namespace Library.ViewModels
{
    public class IndexViewModel
    {
        public List<Book> NewBooks { get; set; }
        
        public List<Book> TopBooks { get; set; }
    }
}

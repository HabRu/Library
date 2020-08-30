using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class ListBookViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        
        public SortViewModel SortViewModel { get; set; }
    }
}

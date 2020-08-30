using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class EditBookViewModel
    {
        
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Authtor { get; set; }
       
        public int Year { get; set; }

        public string Language { get; set; }

        public string Genre { get; set; }

        public string Publisher { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}

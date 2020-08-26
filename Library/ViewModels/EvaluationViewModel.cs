using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class EvaluationViewModel
    {
        public int BookId { get; set; }
        public byte Score { get; set; }
        public string user { get;set; }
    }
}

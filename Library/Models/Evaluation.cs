using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Evaluation
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public byte Average { get; set; }

        public List<string> Users { get; set; }

        public Evaluation()
        {
            Users = null;
        }

    }

}

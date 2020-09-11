using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string NameUser { get; set; }
        
        public string CommentString { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

    }
}

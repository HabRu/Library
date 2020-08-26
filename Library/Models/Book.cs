using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Authtor { get; set; }
        public int Year { get; set; }
        public string Language { get; set; }
        public string Genre { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Evaluation Evaluation { get; set; }
        public Status Status { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public Book()
        {
            Description = "Нет описания";
            Comments = new List<Comment>();
            Evaluation = new Evaluation()
            {
                Average = 0,
                BookId = Id,
                Users = new List<string>()
                
            };
        }
    }
}

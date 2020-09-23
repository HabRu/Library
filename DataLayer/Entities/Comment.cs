using DataLayer.Entities;

namespace Library.Models
{
    public class Comment : IEntity<int>
    {
        public int Id { get; set; }

        public string NameUser { get; set; }
        
        public string CommentString { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

    }
}

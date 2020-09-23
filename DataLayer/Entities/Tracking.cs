using DataLayer.Entities;

namespace Library.Models
{
    public class Tracking : IEntity<int>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

    }
}

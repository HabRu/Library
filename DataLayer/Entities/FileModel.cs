using DataLayer.Entities;

namespace Library.Models
{
    public class FileModel : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}

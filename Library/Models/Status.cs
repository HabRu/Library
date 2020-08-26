using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public enum Status
    {
        Естьвналичии,
        Нетвналичии,
        Забронирован,
        Сдан
    }
    public enum ReserveState
    {
        Забронирован,
        Сдан
    }
    public class Comment
    {
        public int Id { get; set; }
        public string NameUser { get; set; }
        public string CommentString { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
    public class Evaluation
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public byte Average { get; set; }
        public List<string> Users { get; set; }
    }
}

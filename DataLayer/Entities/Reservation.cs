using DataLayer.Entities;
using System;

namespace Library.Models
{
    public class Reservation : IEntity<int>
    {
        public int Id { get; set; }

        public int BookIdentificator { get; set; }

        public Book Book { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime DataBooking { get; set; }

        public DateTime DataSend { get; set; }

        public ReserveState State { get; set; }

        public Reservation()
        {
        }
    }
}

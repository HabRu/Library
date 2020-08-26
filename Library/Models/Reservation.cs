using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int BookIdentificator { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DataBooking { get; set; }
        public DateTime DataSend { get; set;}
        public ReserveState State { get; set; }
        public Reservation()
        {
        }
    }
}

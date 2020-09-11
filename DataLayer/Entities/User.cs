using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class User : IdentityUser
    {
        public string NameUser { get; set; }

        public ICollection<Reservation> ReservUser { get; set; }

        public ICollection<Tracking> TrackingList { get; set; }

        public User()
        {
            ReservUser = new List<Reservation>();
            TrackingList = new List<Tracking>();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace Library.Models
{
    public class User:IdentityUser
    {
        public string NameUser { get; set; }
        public ICollection<Reservation> ReservUser { get; set; }
        public User()
        {
            ReservUser = new List<Reservation>();
        }
    }
}

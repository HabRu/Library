using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Settings
    {
        private readonly IConfiguration configuration;

        public Settings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int TimeReservation => configuration.GetValue<int>("TimeReservation");
        public int RunInterval => configuration.GetValue<int>("RunInterval");
    }
}

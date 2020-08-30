using Microsoft.Extensions.Configuration;

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

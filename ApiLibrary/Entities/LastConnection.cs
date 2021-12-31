using System;

namespace ApiLibrary.Entities
{
    public class LastConnection
    {
        public int Id { get; set; }
        public string DeviceType { get; set; }
        public string BrowserName { get; set; }
        public string PlatformName { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public DateTime ConnectionTime { get; set; }

        public User User { get; set; }
    }
}
using System;

namespace CommonLibrary.Dto
{
    public class LastConnectionDto
    {
        public string DeviceType { get; set; }
        public string BrowserName { get; set; }
        public string PlatformName { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public DateTime ConnectionTime { get; set; }
        public string UserName { get; set; }
    }
}
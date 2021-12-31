﻿using System;
using System.ComponentModel;

namespace UI.Models
{
    public class LastConnectionViewModel
    {
        [DisplayName("Device type")]
        public string DeviceType { get; set; }
        [DisplayName("Browser name")]
        public string BrowserName { get; set; }
        [DisplayName("OS name")]
        public string PlatformName { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        [DisplayName("Connection time")]
        public DateTime ConnectionTime { get; set; }
    }
}
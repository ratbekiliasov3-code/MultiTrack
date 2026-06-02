using System;
using System.Collections.Generic;

namespace MultiTrack.Models
{
    public class DashboardSuTakipViewModel
    {
       public string Username { get; set; } = string.Empty;
        public DateTime SecilenTarih { get; set; } = DateTime.Today;
        public Dictionary<string, double> WaterByDateMl { get; set; } = new Dictionary<string, double>();
    }
}

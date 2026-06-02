using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiTrack.Models
{
    public class DashboardParaTakipViewModel
    {
       public string Username { get; set; } = string.Empty;
        public DateTime Tarih { get; set; } = DateTime.Today;
        public List<Harcama> Expenses { get; set; } = new List<Harcama>();
        public double TodayTotal { get; set; }
        public double MonthlyTotal { get; set; }

        public double TotalAmount => Expenses.Sum(e => e.Tutar);
    }
}

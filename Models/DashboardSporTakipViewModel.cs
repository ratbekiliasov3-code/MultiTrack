using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiTrack.Models
{
    public class DashboardSporTakipViewModel
    {
        public string CurrentDayName { get; set; } = "PAZARTESI";
        public int TodayDayIndex { get; set; } = 0;
        public List<SporAntrenman> Workouts { get; set; } = new List<SporAntrenman>();
        
        // Hesaplanan veriler
        public int TotalWeeklyWorkouts => Workouts.Count;
        public int CompletedWeeklyWorkouts => Workouts.Count(w => w.IsCompleted);
        public double WeeklyCompletionRate => TotalWeeklyWorkouts == 0 ? 0 : (double)CompletedWeeklyWorkouts / TotalWeeklyWorkouts * 100;
    }
}
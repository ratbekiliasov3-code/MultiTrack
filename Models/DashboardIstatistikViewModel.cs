using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiTrack.Models
{
    public class DashboardIstatistikViewModel
    {
       public string Username { get; set; } = string.Empty;

        // Water data for last 7 days
        public List<double> WaterDataLast7Days { get; set; } = new List<double>();
        public List<string> WaterDaysLabels { get; set; } = new List<string>();
        public double WaterAverageLast7Days { get; set; }
        public double WaterTotalLast7Days { get; set; }

        // Sports activity data
        public int TotalWorkoutsThisMonth { get; set; }
        public int CompletedWorkoutsThisMonth { get; set; }
        public double WorkoutCompletionRate { get; set; }

        // Expense data for this month
        public double MonthlyExpenseTotal { get; set; }
        public int ExpenseCountThisMonth { get; set; }
        public double AverageDailyExpense { get; set; }

        // Reading progress
        public string CurrentBook { get; set; } = "";
        public int TotalPages { get; set; }
        public int PagesRead { get; set; }
        public double ReadingProgress { get; set; }

        // Tasks data
        public int TotalTasksThisMonth { get; set; }
        public int CompletedTasksThisMonth { get; set; }
        public double TaskCompletionRate { get; set; }
    }
}

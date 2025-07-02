using HabitTrackerApp.Classes.Services;

namespace HabitTrackerApp.Classes
{
    public class Habit
    {
        public string Name { get; set; }
        public List<DateTime> CompletionDates { get; set; } = new List<DateTime>();

        public Habit(string name)
        {
            Name = name;
        }

        public void MarkComplete()
        {
            DateTime today = DateTime.Today;
            if (!CompletionDates.Contains(today))
            {
                CompletionDates.Add(today);
                Console.WriteLine(string.Format(LocalizationService.GetString("HabitMarkedComplete"), Name, today.ToShortDateString()));
            }
            else
            {
                Console.WriteLine(LocalizationService.GetString("HabitAlreadyCompletedToday"));
            }
        }

        public int CurrentStreak()
        {
            CompletionDates.Sort((a, b) => b.CompareTo(a)); // newest first
            int streak = 0;
            DateTime day = DateTime.Today;

            foreach (var date in CompletionDates)
            {
                if (date == day)
                {
                    streak++;
                    day = day.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            return streak;
        }

        public void ShowHistory()
        {
            Console.WriteLine(string.Format(LocalizationService.GetString("HabitHistoryFor"), Name));
            foreach (var date in CompletionDates.OrderBy(d => d))
            {
                Console.WriteLine(string.Format(LocalizationService.GetString("HabitHistoryDateLine"), date.ToShortDateString()));
            }
        }
    }
}

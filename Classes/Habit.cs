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
                Console.WriteLine($"✅ '{Name}' marked as complete for {today.ToShortDateString()}.");
            }
            else
            {
                Console.WriteLine("⚠️ You've already marked this habit complete today.");
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
            Console.WriteLine($"📅 History for '{Name}':");
            foreach (var date in CompletionDates.OrderBy(d => d))
            {
                Console.WriteLine($"- {date.ToShortDateString()}");
            }
        }
        
    }

}

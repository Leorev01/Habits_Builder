using System.ComponentModel.Design;
using HabitTrackerApp.Classes;

namespace HabitTrackerApp
{
    class Program
    {

        static List<Habit> habits = Storage.Load();
        static ReminderService reminderService = new ReminderService();

        static void Main(string[] args)
        {
            DotNetEnv.Env.Load();

            Console.WriteLine("📅 Welcome to Habit Tracker!");

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Add Habit");
                Console.WriteLine("2. View Habits");
                Console.WriteLine("3. Mark Habit Complete");
                Console.WriteLine("4. View Habit History");
                Console.WriteLine("5. Delete Habit");
                Console.WriteLine("6. Set Email Reminders");
                Console.WriteLine("7. Exit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": AddHabit(); break;
                    case "2": ViewHabits(); break;
                    case "3": MarkHabitComplete(); break;
                    case "4": ViewHabitHistory(); break;
                    case "5": DeleteHabit(); break;
                    case "6": SetEmailReminders(); break;
                    case "7": Storage.Save(habits); reminderService.StopReminders(); return;
                    default: Console.WriteLine("❗ Invalid option."); break;
                }
            }
        }

        static void AddHabit()
        {
            Console.Write("Enter habit name: ");
            var name = Console.ReadLine();
            habits.Add(new Habit(name));
            Console.WriteLine($"➕ Added habit: {name}");
            Storage.Save(habits);
        }

        static void ViewHabits()
        {
            if (habits.Count == 0)
            {
                Console.WriteLine("No habits yet.");
                return;
            }

            Console.WriteLine("\n📋 Your Habits:");
            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {habits[i].Name} - Current streak: {habits[i].CurrentStreak()}");
            }
        }

        static void MarkHabitComplete()
        {
            ViewHabits();
            Console.Write("Select habit number to mark complete: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= habits.Count)
            {
                habits[index - 1].MarkComplete();
                Storage.Save(habits);
            }
            else
            {
                Console.WriteLine("❌ Invalid habit number.");
            }
        }

        static void ViewHabitHistory()
        {
            ViewHabits();
            Console.Write("Select habit number to view history: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= habits.Count)
            {
                habits[index - 1].ShowHistory();
            }
            else
            {
                Console.WriteLine("❌ Invalid habit number.");
            }
        }

        static void DeleteHabit()
        {
            ViewHabits();
            Console.Write("Select habit number to delete: ");
            var value = Console.ReadLine();
            if (int.TryParse(value, out int index) && index >= 1 && index <= habits.Count)
            {
                habits.RemoveAt(index - 1);
                Console.WriteLine($"❌ Deleted habit: {index}");
                Storage.Save(habits);
            }
            else
            {
                Console.WriteLine("❌ Invalid habit number.");
            }
        }

        static void SetEmailReminders()
        {
            Console.WriteLine("What time would you like to receive your reminder? \nMust set it in HH:mm format.");
            Console.Write("Enter time: ");
            var input = Console.ReadLine();
            if (!TimeSpan.TryParse(input, out TimeSpan reminderTime))
            {
                Console.WriteLine("❌ Invalid time format.");
                return;
            }
            reminderService.SendReminders(habits, reminderTime);
            Console.WriteLine($"⏰ Reminder set for {reminderTime}.");

        }
    }
}

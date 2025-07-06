using HabitTrackerApp.Classes;
using HabitTrackerApp.Classes.Services;
using HabitTrackerApp.Services; // Add this using statement for ThemeService

namespace HabitTrackerApp
{
    class Program
    {
        static List<Habit> habits = Storage.Load();
        static ReminderService reminderService = new ReminderService();

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Apply the saved theme when the application starts
            ThemeService.ApplySavedTheme();

            await LanguageService.LoadLanguagesAsync();
            await LocalizationService.InitializeAsync();

            DotNetEnv.Env.Load();

            Console.WriteLine(LocalizationService.GetString("WelcomeMessage"));

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(LocalizationService.GetString("ChooseOption"));
                Console.WriteLine();

                Console.WriteLine(LocalizationService.GetString("MenuAddHabit"));
                Console.WriteLine(LocalizationService.GetString("MenuViewHabits"));
                Console.WriteLine(LocalizationService.GetString("MenuMarkComplete"));
                Console.WriteLine(LocalizationService.GetString("MenuViewHistory"));
                Console.WriteLine(LocalizationService.GetString("MenuDeleteHabit"));
                Console.WriteLine(LocalizationService.GetString("MenuSetReminders"));
                Console.WriteLine(LocalizationService.GetString("MenuChangeLanguage"));
                // New menu option for toggling theme
                Console.WriteLine($"{LocalizationService.GetString("MenuToggleTheme")} {ThemeService.GetThemeIcon()}");
                // Updated Exit option
                Console.WriteLine(LocalizationService.GetString("MenuExit"));

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": AddHabit(); break;
                    case "2": ViewHabits(); break;
                    case "3": MarkHabitComplete(); break;
                    case "4": ViewHabitHistory(); break;
                    case "5": DeleteHabit(); break;
                    case "6": SetEmailReminders(); break;
                    case "7": await ChangeLanguageAsync(); break;
                    case "8": // New case for toggling theme
                        ThemeService.ToggleTheme();
                        // After toggling, we might want to re-display the menu
                        // to show the updated theme icon immediately.
                        // The while(true) loop will handle this.
                        break;
                    case "9": // Updated case for Exit
                        Storage.Save(habits);
                        reminderService.StopReminders();
                        return;
                    default:
                        Console.WriteLine(LocalizationService.GetString("InvalidOption"));
                        break;
                }
            }
        }

        static async Task ChangeLanguageAsync()
        {
            var languages = LanguageService.GetLanguages();

            Console.WriteLine(LocalizationService.GetString("MenuChangeLanguage").Replace("7.", "").Trim() + ":");

            int i = 1;
            foreach (var lang in languages)
            {
                // Display languages in format: 1. English (en)
                Console.WriteLine($"{i}. {lang.Value} ({lang.Key})");
                i++;
            }

            var input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= languages.Count)
            {
                string selectedCode = languages.ElementAt(choice - 1).Key;
                await LocalizationService.SetLanguageAsync(selectedCode);
                // After changing language, re-apply theme to ensure all new strings are displayed with correct colors
                ThemeService.ApplySavedTheme();
            }
            else
            {
                Console.WriteLine(LocalizationService.GetString("InvalidOption"));
            }
        }

        static void AddHabit()
        {
            Console.Write(LocalizationService.GetString("EnterHabitName"));
            var name = Console.ReadLine();
            habits.Add(new Habit(name));
            Console.WriteLine(string.Format(LocalizationService.GetString("AddedHabit"), name));
            Storage.Save(habits);
        }

        static void ViewHabits()
        {
            if (habits.Count == 0)
            {
                Console.WriteLine(LocalizationService.GetString("NoHabitsYet"));
                return;
            }

            Console.WriteLine();
            Console.WriteLine(LocalizationService.GetString("YourHabits"));
            for (int i = 0; i < habits.Count; i++)
            {
                var line = string.Format(
                    LocalizationService.GetString("HabitLineFormat"),
                    i + 1,
                    habits[i].Name,
                    habits[i].CurrentStreak()
                );
                Console.WriteLine(line);
            }
        }

        static void MarkHabitComplete()
        {
            ViewHabits();
            Console.Write(LocalizationService.GetString("SelectHabitComplete"));
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= habits.Count)
            {
                habits[index - 1].MarkComplete();
                Storage.Save(habits);
            }
            else
            {
                Console.WriteLine(LocalizationService.GetString("InvalidHabitNumber"));
            }
        }

        static void ViewHabitHistory()
        {
            ViewHabits();
            Console.Write(LocalizationService.GetString("SelectHabitHistory"));
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= habits.Count)
            {
                habits[index - 1].ShowHistory();
            }
            else
            {
                Console.WriteLine(LocalizationService.GetString("InvalidHabitNumber"));
            }
        }

        static void DeleteHabit()
        {
            ViewHabits();
            Console.Write(LocalizationService.GetString("SelectHabitDelete"));
            var value = Console.ReadLine();
            if (int.TryParse(value, out int index) && index >= 1 && index <= habits.Count)
            {
                habits.RemoveAt(index - 1);
                Console.WriteLine(string.Format(LocalizationService.GetString("DeletedHabit"), index));
                Storage.Save(habits);
            }
            else
            {
                Console.WriteLine(LocalizationService.GetString("InvalidHabitNumber"));
            }
        }

        static void SetEmailReminders()
        {
            Console.WriteLine(LocalizationService.GetString("ReminderTimePrompt"));
            Console.Write(LocalizationService.GetString("EnterTime"));
            var input = Console.ReadLine();
            if (!TimeSpan.TryParse(input, out TimeSpan reminderTime))
            {
                Console.WriteLine(LocalizationService.GetString("InvalidTimeFormat"));
                return;
            }
            reminderService.SendReminders(habits, reminderTime);
            Console.WriteLine(string.Format(LocalizationService.GetString("ReminderSet"), reminderTime));
        }
    }
}
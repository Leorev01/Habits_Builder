using HabitTrackerApp.Classes;
using HabitTrackerApp.Classes.Services;
using HabitTrackerApp.Services;

namespace HabitTrackerApp
{
    class Program
    {
        private static GoogleCalendarService _calendarService = new GoogleCalendarService();
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
                Console.WriteLine($"{LocalizationService.GetString("MenuToggleTheme")} {ThemeService.GetThemeIcon()}");
                Console.WriteLine($"{LocalizationService.GetString("MenuSyncCalendar")} {(_calendarService.IsInitialized ? "✅" : "🔒")}");
                Console.WriteLine(LocalizationService.GetString("MenuExit"));

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": 
                        AddHabit(); 
                        break;
                    case "2": 
                        ViewHabits(); 
                        break;
                    case "3": 
                        MarkHabitComplete(); 
                        break;
                    case "4": 
                        ViewHabitHistory(); 
                        break;
                    case "5": 
                        DeleteHabit(); 
                        break;
                    case "6": 
                        SetEmailReminders(); 
                        break;
                    case "7": 
                        await ChangeLanguageAsync(); 
                        break;
                    case "8": 
                        ThemeService.ToggleTheme();
                        break;
                    case "9": 
                        await SyncToGoogleCalendar();
                        break;
                    case "10": 
                        await ExitApplication();
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

            Console.WriteLine();
            Console.WriteLine(LocalizationService.GetString("MenuChangeLanguage").Replace("7.", "").Trim() + ":");
            Console.WriteLine();

            int i = 1;
            foreach (var lang in languages)
            {
                Console.WriteLine($"{i}. {lang.Value} ({lang.Key})");
                i++;
            }
            Console.WriteLine();

            Console.WriteLine();
            Console.Write(LocalizationService.GetString("SelectLanguage"));
            var input = Console.ReadLine();
            
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= languages.Count)
            {
                string selectedCode = languages.ElementAt(choice - 1).Key;
                await LocalizationService.SetLanguageAsync(selectedCode);
                ThemeService.ApplySavedTheme();
                Console.WriteLine($"✅ {LocalizationService.GetString("LanguageChanged")}");
            }
            else
            {
                Console.WriteLine(LocalizationService.GetString("InvalidOption"));
            }
        }

        static void AddHabit()
        {
            Console.WriteLine();
            Console.Write(LocalizationService.GetString("EnterHabitName"));
            var name = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine(LocalizationService.GetString("HabitNameRequired"));
                return;
            }

            habits.Add(new Habit(name));
            Console.WriteLine(string.Format(LocalizationService.GetString("AddedHabit"), name));
            Storage.Save(habits);
        }

        static void ViewHabits()
        {
            Console.WriteLine();
            if (habits.Count == 0)
            {
                Console.WriteLine(LocalizationService.GetString("NoHabitsYet"));
                return;
            }

            Console.WriteLine(LocalizationService.GetString("YourHabits"));
            Console.WriteLine(new string('-', 50));
            
            for (int i = 0; i < habits.Count; i++)
            {
                var streak = habits[i].CurrentStreak();
                var streakEmoji = streak > 0 ? "🔥" : "⭕";
                var line = string.Format(
                    LocalizationService.GetString("HabitLineFormat"),
                    i + 1,
                    habits[i].Name,
                    streak,
                    streakEmoji
                );
                Console.WriteLine(line);
            }
            Console.WriteLine(new string('-', 50));
        }

        static void MarkHabitComplete()
        {
            ViewHabits();
            
            if (habits.Count == 0)
                return;

            Console.WriteLine();
            Console.Write(LocalizationService.GetString("SelectHabitComplete"));
            var input = Console.ReadLine();
            
            if (int.TryParse(input, out int index) && index >= 1 && index <= habits.Count)
            {
                var habit = habits[index - 1];
                habit.MarkComplete();
                Storage.Save(habits);
                Console.WriteLine(string.Format(LocalizationService.GetString("HabitMarkedComplete"), habit.Name));
            }
            else
            {
                Console.WriteLine(LocalizationService.GetString("InvalidHabitNumber"));
            }
        }

        static void ViewHabitHistory()
        {
            ViewHabits();
            
            if (habits.Count == 0)
                return;

            Console.WriteLine();
            Console.Write(LocalizationService.GetString("SelectHabitHistory"));
            var input = Console.ReadLine();
            
            if (int.TryParse(input, out int index) && index >= 1 && index <= habits.Count)
            {
                Console.WriteLine();
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
            
            if (habits.Count == 0)
                return;

            Console.WriteLine();
            Console.Write(LocalizationService.GetString("SelectHabitDelete"));
            var input = Console.ReadLine();
            
            if (int.TryParse(input, out int index) && index >= 1 && index <= habits.Count)
            {
                var habitName = habits[index - 1].Name;
                
                Console.Write(string.Format(LocalizationService.GetString("ConfirmDelete"), habitName));
                var confirmation = Console.ReadLine()?.ToLower();
                
                if (confirmation == "y" || confirmation == "yes" || confirmation == LocalizationService.GetString("Yes").ToLower())
                {
                    habits.RemoveAt(index - 1);
                    Console.WriteLine(string.Format(LocalizationService.GetString("DeletedHabit"), habitName));
                    Storage.Save(habits);
                }
                else
                {
                    Console.WriteLine(LocalizationService.GetString("DeleteCancelled"));
                }
            }
            else
            {
                Console.WriteLine(LocalizationService.GetString("InvalidHabitNumber"));
            }
        }

        static void SetEmailReminders()
        {
            Console.WriteLine();
            Console.WriteLine(LocalizationService.GetString("ReminderTimePrompt"));
            Console.Write(LocalizationService.GetString("EnterTime"));
            var input = Console.ReadLine();
            
            if (!TimeSpan.TryParse(input, out TimeSpan reminderTime))
            {
                Console.WriteLine(LocalizationService.GetString("InvalidTimeFormat"));
                return;
            }

            try
            {
                reminderService.SendReminders(habits, reminderTime);
                Console.WriteLine(string.Format(LocalizationService.GetString("ReminderSet"), reminderTime));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ {LocalizationService.GetString("ReminderSetupFailed")}: {ex.Message}");
            }
        }

        static async Task SyncToGoogleCalendar()
        {
            Console.WriteLine();
            
            if (habits.Count == 0)
            {
                Console.WriteLine(LocalizationService.GetString("NoHabitsToSync"));
                return;
            }

            // Initialize Google Calendar only when needed
            if (!_calendarService.IsInitialized)
            {
                Console.WriteLine("🔐 " + LocalizationService.GetString("ConnectingToCalendar"));
                try
                {
                    bool initialized = await _calendarService.InitializeAsync();
                    if (!initialized)
                    {
                        Console.WriteLine("❌ " + LocalizationService.GetString("CalendarConnectionFailed"));
                        return;
                    }
                    Console.WriteLine("✅ " + LocalizationService.GetString("CalendarConnected"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ {LocalizationService.GetString("CalendarConnectionFailed")}: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine(LocalizationService.GetString("CalendarSyncPrompt"));
            Console.Write(LocalizationService.GetString("EnterDateOrToday"));
            string input = Console.ReadLine();

            DateTime targetDate = DateTime.Today;
            if (!string.IsNullOrEmpty(input) && DateTime.TryParse(input, out DateTime parsed))
            {
                targetDate = parsed;
            }

            Console.WriteLine();
            Console.WriteLine(string.Format(LocalizationService.GetString("SyncingToCalendar"), targetDate.ToString("yyyy-MM-dd")));
            
            int successCount = 0;
            int failureCount = 0;

            // Show progress
            for (int i = 0; i < habits.Count; i++)
            {
                Console.Write($"\r{LocalizationService.GetString("SyncingHabit")} {i + 1}/{habits.Count}: {habits[i].Name}...");
                
                try
                {
                    bool success = await _calendarService.CreateHabitEventAsync(habits[i], targetDate);
                    if (success)
                    {
                        successCount++;
                    }
                    else
                    {
                        failureCount++;
                        Console.WriteLine($"\n❌ {LocalizationService.GetString("SyncFailedFor")} {habits[i].Name}");
                    }
                }
                catch (Exception ex)
                {
                    failureCount++;
                    Console.WriteLine($"\n❌ {LocalizationService.GetString("SyncFailedFor")} {habits[i].Name}: {ex.Message}");
                }
                
                // Small delay to prevent API rate limiting
                await Task.Delay(500);
            }

            Console.WriteLine();
            if (successCount > 0)
            {
                Console.WriteLine($"✅ {string.Format(LocalizationService.GetString("CalendarSyncComplete"), successCount, targetDate.ToString("yyyy-MM-dd"))}");
            }
            
            if (failureCount > 0)
            {
                Console.WriteLine($"⚠️ {string.Format(LocalizationService.GetString("CalendarSyncPartialFailure"), failureCount)}");
            }
        }

        static async Task ExitApplication()
        {
            Console.WriteLine();
            Console.WriteLine(LocalizationService.GetString("SavingData"));
            
            try
            {
                Storage.Save(habits);
                reminderService.StopReminders();
                Console.WriteLine(LocalizationService.GetString("DataSaved"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {LocalizationService.GetString("SaveError")}: {ex.Message}");
            }
            
            Console.WriteLine(LocalizationService.GetString("GoodbyeMessage"));
            await Task.Delay(1000); // Brief pause for user to see the message
        }
    }
}
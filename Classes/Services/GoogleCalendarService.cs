using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;

namespace HabitTrackerApp.Classes
{
    public class GoogleCalendarService
    {
        private static readonly string[] Scopes = { CalendarService.Scope.Calendar };
        private static readonly string ApplicationName = "Habit Tracker App";
        private CalendarService _service;
        private bool _isInitialized = false;

        public async Task<bool> InitializeAsync()
        {
            if (_isInitialized && _service != null)
                return true;

            try
            {
                UserCredential credential;
                using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore("token.json", true));
                }

                _service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                _isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Google Calendar initialization failed: {ex.Message}");
                return false;
            }
        }

        public bool IsInitialized => _isInitialized;

        public async Task<bool> CreateHabitEventAsync(Habit habit, DateTime date)
        {
            if (!_isInitialized)
                return false;

            try
            {
                // Check if event already exists to avoid duplicates
                var existingEvents = await GetEventsForDateAsync(date);
                if (existingEvents.Any(e => e.Summary == $"📌 {habit.Name}"))
                {
                    Console.WriteLine($"⚠️ Event already exists for {habit.Name} on {date:yyyy-MM-dd}");
                    return true; // Consider this a success
                }

                var eventRequest = new Event()
                {
                    Summary = $"📌 {habit.Name}",
                    Description = $"Habit tracking: {habit.Name}\n\nCreated by Habit Tracker App",
                    Start = new EventDateTime()
                    {
                        Date = date.ToString("yyyy-MM-dd"), // All-day event format
                    },
                    End = new EventDateTime()
                    {
                        Date = date.AddDays(1).ToString("yyyy-MM-dd"), // All-day event ends next day
                    },
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = false,
                        Overrides = new EventReminder[]
                        {
                            new EventReminder() { Method = "popup", Minutes = 480 }, // 8 hours before (morning reminder)
                            new EventReminder() { Method = "email", Minutes = 1440 } // 24 hours before (day before reminder)
                        }
                    },
                    ColorId = "2" // Green color for habit events
                };

                await _service.Events.Insert(eventRequest, "primary").ExecuteAsync();
                Console.WriteLine($"✅ Created all-day calendar event for {habit.Name}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to create calendar event for {habit.Name}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SyncHabitsToCalendarAsync(List<Habit> habits, DateTime targetDate)
        {
            if (!_isInitialized)
                return false;

            try
            {
                int successCount = 0;
                int totalHabits = habits.Count;

                Console.WriteLine($"🔄 Starting sync of {totalHabits} habits to calendar...");

                foreach (var habit in habits)
                {
                    bool success = await CreateHabitEventAsync(habit, targetDate);
                    if (success) 
                    {
                        successCount++;
                    }
                    
                    // Small delay to prevent API rate limiting
                    await Task.Delay(300);
                }

                if (successCount == totalHabits)
                {
                    Console.WriteLine($"✅ Successfully synced all {successCount} habits to calendar for {targetDate:yyyy-MM-dd}");
                }
                else if (successCount > 0)
                {
                    Console.WriteLine($"⚠️ Partially synced {successCount}/{totalHabits} habits to calendar");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to sync any habits to calendar");
                }

                return successCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to sync habits to calendar: {ex.Message}");
                return false;
            }
        }

        private async Task<List<Event>> GetEventsForDateAsync(DateTime date)
        {
            try
            {
                var request = _service.Events.List("primary");
                request.TimeMin = date.Date;
                request.TimeMax = date.Date.AddDays(1);
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                var events = await request.ExecuteAsync();
                return events.Items?.ToList() ?? new List<Event>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Could not check existing events: {ex.Message}");
                return new List<Event>();
            }
        }

        public async Task<bool> DeleteHabitEventAsync(string habitName, DateTime date)
        {
            if (!_isInitialized)
                return false;

            try
            {
                var events = await GetEventsForDateAsync(date);
                var habitEvent = events.FirstOrDefault(e => e.Summary == $"📌 {habitName}");

                if (habitEvent != null)
                {
                    await _service.Events.Delete("primary", habitEvent.Id).ExecuteAsync();
                    Console.WriteLine($"✅ Deleted calendar event for {habitName}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"⚠️ No calendar event found for {habitName} on {date:yyyy-MM-dd}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to delete calendar event for {habitName}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Event>> GetHabitEventsAsync(DateTime startDate, DateTime endDate)
        {
            if (!_isInitialized)
                return new List<Event>();

            try
            {
                var request = _service.Events.List("primary");
                request.TimeMin = startDate.Date;
                request.TimeMax = endDate.Date.AddDays(1);
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
                request.Q = "📌"; // Search for habit events

                var events = await request.ExecuteAsync();
                return events.Items?.Where(e => e.Summary?.StartsWith("📌") == true).ToList() ?? new List<Event>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to retrieve habit events: {ex.Message}");
                return new List<Event>();
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            if (!_isInitialized)
                return false;

            try
            {
                var calendarList = await _service.CalendarList.List().ExecuteAsync();
                Console.WriteLine($"✅ Successfully connected to Google Calendar");
                Console.WriteLine($"📅 Found {calendarList.Items?.Count ?? 0} calendars");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Connection test failed: {ex.Message}");
                return false;
            }
        }
    }
}
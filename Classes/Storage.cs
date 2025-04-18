using System.IO;
using System.Text.Json;

namespace HabitTrackerApp.Classes
{
    static class Storage
    {
        private static string filePath = "habits.json";

        public static void Save(List<Habit> habits)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(filePath, JsonSerializer.Serialize(habits, options));
        }

        public static List<Habit> Load()
        {
            if (!File.Exists(filePath))
                return new List<Habit>();

            return JsonSerializer.Deserialize<List<Habit>>(File.ReadAllText(filePath)) ?? new List<Habit>();
        }
    } 
}


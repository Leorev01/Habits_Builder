using System.Diagnostics;
using System.Text.Json;

namespace HabitTrackerApp.Classes.Services
{
    public static class SettingsService
    {
        // Use the full path to the settings file
        private static readonly string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HabitTrackerApp");
        private static readonly string SettingsFilePath;

        static SettingsService()
        {
            if (!Directory.Exists(appDataDir))
                Directory.CreateDirectory(appDataDir);

            SettingsFilePath = Path.Combine(appDataDir, "settings.json");
        }

        // Save settings
        public static void SaveSetting<T>(string key, T value)
        {
            try
            {
                // Load current settings
                var settings = LoadSettings();

                // Update or add new value
                settings[key] = JsonSerializer.Serialize(value);

                // Save updated settings to file
                File.WriteAllText(SettingsFilePath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));

                Debug.WriteLine($"Setting saved: {key} = {value}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        // Load setting by key
        public static T? LoadSetting<T>(string key, T? defaultValue = default)
        {
            try
            {
                // Load all settings
                var settings = LoadSettings();

                // If key exists, deserialize value
                if (settings.TryGetValue(key, out var value))
                {
                    Debug.WriteLine($"Loaded setting: {key} = {value}");
                    return JsonSerializer.Deserialize<T>(value);
                }

                Debug.WriteLine($"Setting for key '{key}' not found. Using default value: {defaultValue}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading settings: {ex.Message}");
            }

            // Return default value if something went wrong
            return defaultValue;
        }

        // Load all settings from file
        private static Dictionary<string, string> LoadSettings()
        {
            try
            {
                // If file exists, load it
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }

                Debug.WriteLine("Settings file not found. Using empty dictionary.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading settings: {ex.Message}");
            }

            // If file is missing or error occurred, return empty dictionary
            return new Dictionary<string, string>();
        }
    }
}

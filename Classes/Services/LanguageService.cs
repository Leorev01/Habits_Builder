using System.Text.Json;

namespace HabitTrackerApp.Classes.Services
{
    public static class LanguageService
    {
        private static Dictionary<string, string> _languages = new();

        public static async Task LoadLanguagesAsync()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Languages.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Languages.json не знайдено.");
                _languages = new Dictionary<string, string>();
                return;
            }

            using var stream = File.OpenRead(filePath);
            var dict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
            _languages = dict ?? new Dictionary<string, string>();
        }

        public static IReadOnlyDictionary<string, string> GetLanguages()
        {
            return _languages;
        }

        public static bool IsLanguageSupported(string code)
        {
            return _languages.ContainsKey(code);
        }
    }
}

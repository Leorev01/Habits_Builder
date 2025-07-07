using System.Globalization;
using System.Text.Json;

namespace HabitTrackerApp.Classes.Services
{
    public static class LocalizationService
    {
        private static readonly object _lock = new();
        private static Dictionary<string, string> _localizedStrings = new();
        private static Dictionary<string, string> _fallbackStrings = new();

        private static CultureInfo _currentCulture;

        // List of available languages (CultureInfo)
        private static readonly List<CultureInfo> AvailableCultures = new()
        {
            new CultureInfo("en"),
            new CultureInfo("uk"),
            new CultureInfo("fr"),
            new CultureInfo("ru"),
            new CultureInfo("it"),
        };

        public static event Action? LanguageChanged;

        public static CultureInfo CurrentCulture
        {
            get => _currentCulture;
            private set
            {
                lock (_lock)
                {
                    _currentCulture = value;
                }
            }
        }

        // Initialization: load culture from settings and localization
        public static async Task InitializeAsync()
        {
            var savedCultureName = SettingsService.LoadSetting<string>("Language") ?? "en";
            var culture = new CultureInfo(savedCultureName);
            if (!AvailableCultures.Contains(culture))
                culture = new CultureInfo("en");

            await SetLanguageAsync(culture.Name);
        }

        public static async Task SetLanguageAsync(string cultureName)
        {
            if (!LanguageService.IsLanguageSupported(cultureName))
            {
                Console.WriteLine($"Language '{cultureName}' is not supported. Using English.");
                cultureName = "en";
            }

            var culture = new CultureInfo(cultureName);

            var localized = await LoadLocalizationAsync(culture.Name);
            var fallback = await LoadLocalizationAsync("en");

            lock (_lock)
            {
                _localizedStrings = localized;
                _fallbackStrings = fallback;
                _currentCulture = culture;
                SettingsService.SaveSetting("Language", culture.Name);
            }

            LanguageChanged?.Invoke();
        }

        private static async Task<Dictionary<string, string>> LoadLocalizationAsync(string cultureName)
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Localizations", $"{cultureName}.json");
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Localization file {cultureName}.json not found.");
                    return new Dictionary<string, string>();
                }

                using var stream = File.OpenRead(filePath);
                var dict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
                return dict ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading localization {cultureName}: {ex.Message}");
                return new Dictionary<string, string>();
            }
        }

        public static string GetString(string key)
        {
            lock (_lock)
            {
                if (_localizedStrings.TryGetValue(key, out var localized))
                    return localized;

                if (_fallbackStrings.TryGetValue(key, out var fallback))
                    return fallback;

                // If the key is not found, return the key itself — to make it visible
                return key;
            }
        }

        internal static List<string> GetAvailableLanguages()
        {
            return AvailableCultures.Select(c => c.EnglishName).ToList();
        }

    }
}

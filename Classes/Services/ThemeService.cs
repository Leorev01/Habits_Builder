using HabitTrackerApp.Classes.Services; // Assuming SettingsService is in this namespace
using System.Diagnostics;

namespace HabitTrackerApp.Services // Changed namespace to match your project's structure
{
    public static class ThemeService
    {
        private const string IsDarkModeKey = "IsDarkMode"; // Key for saving the dark mode state

        // Event to notify about theme changes
        public static event Action? ThemeChanged;

        /// <summary>
        /// Toggles the current theme between light and dark.
        /// Saves the choice and applies the new console colors.
        /// </summary>
        public static void ToggleTheme()
        {
            bool currentIsDarkMode = LoadIsDarkMode();
            bool newIsDarkMode = !currentIsDarkMode; // Toggle the state

            SaveIsDarkMode(newIsDarkMode); // Save the new state

            ApplyTheme(newIsDarkMode); // Apply the new theme to the console

            OnThemeChanged(); // Notify about the theme change (if there are subscribers)
        }

        /// <summary>
        /// Applies the current saved theme to the console.
        /// This method should be called when the application starts.
        /// </summary>
        public static void ApplySavedTheme()
        {
            bool isDarkMode = LoadIsDarkMode();
            ApplyTheme(isDarkMode);
        }

        /// <summary>
        /// Applies the specified theme (dark or light) to the console.
        /// </summary>
        /// <param name="isDarkMode">True for dark theme, False for light theme.</param>
        private static void ApplyTheme(bool isDarkMode)
        {
            if (isDarkMode)
            {
                // Dark theme: white text on black background
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                // Light theme: black text on white background (or default)
                // You can also set standard colors if known, or reset
                Console.BackgroundColor = ConsoleColor.White; // e.g., white background
                Console.ForegroundColor = ConsoleColor.Black;  // e.g., black text
                // Alternative for returning to system colors: Console.ResetColor();
                // But for a "light" theme, it's better to explicitly set to be sure
            }
            Console.Clear(); // Clear the console for changes to apply to the entire window
            Debug.WriteLine($"Console theme set to: {(isDarkMode ? "Dark" : "Light")}");
        }

        /// <summary>
        /// Saves the dark mode state.
        /// </summary>
        /// <param name="isDarkMode">True if the theme is dark, False if light.</param>
        private static void SaveIsDarkMode(bool isDarkMode)
        {
            try
            {
                Debug.WriteLine($"Saving dark mode state: {isDarkMode}");
                SettingsService.SaveSetting(IsDarkModeKey, isDarkMode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving dark mode state: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the dark mode state.
        /// </summary>
        /// <returns>True if the saved theme is dark; False if light or not found.</returns>
        public static bool LoadIsDarkMode()
        {
            try
            {
                // Load the value, default to False (light theme)
                bool? savedIsDarkMode = SettingsService.LoadSetting<bool?>(IsDarkModeKey, false);

                if (savedIsDarkMode.HasValue)
                {
                    Debug.WriteLine($"Loaded dark mode state: {savedIsDarkMode.Value}");
                    return savedIsDarkMode.Value;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading dark mode state: {ex.Message}");
            }

            Debug.WriteLine("Dark mode state not found. Defaulting to: Light.");
            return false; // Default to light theme
        }

        /// <summary>
        /// Returns an icon corresponding to the current theme.
        /// </summary>
        /// <returns>A string with an icon (e.g., "☀️" or "🌙").</returns>
        public static string GetThemeIcon()
        {
            return LoadIsDarkMode() ? "☀️" : "🌙"; // Sun for dark (to switch to light), moon for light (to switch to dark)
        }

        /// <summary>
        /// Invokes the ThemeChanged event.
        /// </summary>
        private static void OnThemeChanged()
        {
            ThemeChanged?.Invoke();
        }
    }
}
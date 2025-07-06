# Dark Mode

This document details the implementation and usage of the dark mode feature in the "Habit Tracker App" console application.

---

## Feature Overview

To enhance the user experience, especially when working in low-light environments, the console application now includes **Dark Mode** support. This feature allows users to switch the console's color scheme between light and dark themes. Your chosen theme is automatically saved, so the application will launch with your last selected theme.

---

## Technical Implementation

Dark mode is implemented using a separate static service, [`ThemeService`](../HabitTrackerApp/Services/ThemeService.cs), which leverages the existing [`SettingsService`](../HabitTrackerApp/Classes/Services/SettingsService.cs) for saving and loading user theme preferences.

### **[`ThemeService.cs`](../HabitTrackerApp/Services/ThemeService.cs)**

* **Separation of Concerns:** `ThemeService` is solely responsible for managing the console's theme logic, using `Console.ForegroundColor` and `Console.BackgroundColor` to set the colors.
* **Persistent Settings:** For persistent storage of the theme state (whether dark mode `bool IsDarkMode` is active), `ThemeService` interacts with `SettingsService`, which serializes and deserializes settings to a JSON file (`settings.json`).
* **Methods:**
    * `ToggleTheme()`: Toggles the current theme state (light/dark), saves it, and immediately applies it to the console.
    * `ApplySavedTheme()`: Applies the last saved theme when the application starts. This method is called at the beginning of the `Main` method in `Program.cs`.
    * `LoadIsDarkMode()` / `SaveIsDarkMode()`: Internal methods for interacting with `SettingsService` to read/write the `IsDarkMode` state.
    * `GetThemeIcon()`: Returns an appropriate icon (☀️ or 🌙) that indicates the current theme state, for use in the menu interface.
* **`ThemeChanged` Event:** Although not critical in the current console implementation (as changes are applied immediately via `Console.Clear()`), this event can be useful for notifying other program components about theme changes if future features require dynamic UI updates based on the theme.

### **[`Program.cs` Integration](../HabitTrackerApp/Program.cs)**

1.  **Initialization:** A call to `ThemeService.ApplySavedTheme();` has been added at the beginning of the `Main` method to load and apply the saved theme on application startup.
2.  **Menu Item:** A new item has been added to the main menu loop:
    * `Console.WriteLine($"{LocalizationService.GetString("MenuToggleTheme")} {ThemeService.GetThemeIcon()}");`
    * This line displays the localized text for toggling the theme along with the corresponding icon.
3.  **Input Handling:** A new `case` (e.g., `case "8":`) has been added to the `switch` statement, which calls `ThemeService.ToggleTheme();` to change the theme.
4.  **Numbering Update:** The "Exit" option's number was incremented to make room for "Toggle Theme."

### **Localization**

To support multi-language functionality, new localization strings for "MenuToggleTheme" have been added to `en.json`, `uk.json`, `fr.json`, and `ru.json` files. This ensures the menu item's name is displayed in the user's selected language. You can find more details on localization in the [Localization Guide](LOCALIZATION_GUIDE.md).

---

## User Guide

1.  **Launch the Application:** Start the Habit Tracker console application by running `dotnet run` from the project root or executing the compiled executable.
2.  **Toggle the Theme:** In the main menu, you will see an option:
    * `8. Toggle Theme ☀️` (if currently dark theme and will switch to light)
    * `8. Toggle Theme 🌙` (if currently light theme and will switch to dark)
    Enter the number `8` and press Enter. The console's color scheme will change immediately.
3.  **Saving:** Your theme choice is automatically saved and will be applied on subsequent application launches.

---
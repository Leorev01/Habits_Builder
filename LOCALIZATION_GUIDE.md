# Developer Guide: Adding New Languages

This guide explains how to add support for a new language to the Habit Tracker console application.

---

## 1. Add the language to `Languages.json`

The `Languages.json` file is located in the `Resources` folder.  
It contains the list of supported languages.

Example format in `Languages.json`:

```json
{
  "en": "English",
  "uk": "Ukrainian",
  "fr": "French",
  "es": "Spanish"  // add new languages here
}
```

- Key: ISO 639-1 two-letter language code (e.g. "en", "uk", "fr", "es").

- Value: Name of the language in English. This ensures that the language selection menu always shows languages in Latin characters.

## 2. Add a localization file for the new language

In the Resources/Localizations folder, create a new JSON file named after the language code, e.g. es.json for Spanish.

The content of this file must be a key-value dictionary where:
- Key: is a unique identifier used in the app.
- Value: is the translated text for that language.

## 3. Example of a localization file
Below is an example based on the existing English (en.json) file:

```json
{
  "WelcomeMessage": "📅 Welcome to Habit Tracker!",
  "ChooseOption": "Choose an option:",
  "MenuAddHabit": "1. Add Habit",
  "MenuViewHabits": "2. View Habits",
  "MenuMarkComplete": "3. Mark Habit Complete",
  "MenuViewHistory": "4. View Habit History",
  "MenuDeleteHabit": "5. Delete Habit",
  "MenuSetReminders": "6. Set Email Reminders",
  "MenuChangeLanguage": "7. Change Language",
  "MenuExit": "8. Exit",
  "InvalidOption": "❗ Invalid option.",
  "EnterHabitName": "Enter habit name: ",
  "AddedHabit": "➕ Added habit: {0}",
  "NoHabitsYet": "No habits yet.",
  "YourHabits": "📋 Your Habits:",
  "HabitLineFormat": "{0}. {1} - Current streak: {2}",
  "SelectHabitComplete": "Select habit number to mark complete: ",
  "InvalidHabitNumber": "❌ Invalid habit number.",
  "SelectHabitHistory": "Select habit number to view history: ",
  "SelectHabitDelete": "Select habit number to delete: ",
  "DeletedHabit": "❌ Deleted habit: {0}",
  "ReminderTimePrompt": "What time would you like to receive your reminder? \nMust set it in HH:mm format.",
  "EnterTime": "Enter time: ",
  "InvalidTimeFormat": "❌ Invalid time format.",
  "ReminderSet": "⏰ Reminder set for {0}.",
  "ReminderWaiting": "⏳ Waiting {0:F1} min until next reminder at {1}",
  "ReminderCancelled": "⏹️ Reminder cancelled.",
  "ReminderCancelledAfterSend": "⏹️ Reminder cancelled after first send.",
  "MissingEmailCredentials": "❌ Missing email credentials.",
  "EmailSubject": "Daily Habit Reminder",
  "EmailSent": "✅ Email sent at {0:T}",
  "EmailBodyHeader": "📌 Your habits for today:\n\n",
  "EmailBodyHabitLine": "- {0}",
  "HabitMarkedComplete": "✅ «{0}» marked as complete for {1}.",
  "HabitAlreadyCompletedToday": "⚠️ You've already marked this habit complete today.",
  "HabitHistoryFor": "📅 History for «{0}»:",
  "HabitHistoryDateLine": "- {0}"
}
```

4. Keeping localization files up to date
- Whenever you add new features or messages to the app, be sure to update all existing localization files to include the new keys and their translations.
- The application always uses these keys to display text. If a key is missing in a specific language file, it will fallback to English (or display the key itself if not found).

5. Testing
- After adding a language, run the app and test switching to the new language.
- Make sure all menu items, prompts, and system messages are displayed correctly.

✅ That’s it!
If you need a template JSON file or examples, look in the existing en.json or ask the team for the latest localization templates.
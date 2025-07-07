# 📅 Habit Tracker App

A comprehensive C# console application to help you build and maintain daily habits. Track streaks, view progress history, receive daily email reminders, and sync your habits with Google Calendar for seamless integration across all your devices.

---

## ✨ Features

- ✅ Add and delete daily habits
- 📈 View current streaks and habit history
- ✅ Mark habits as completed
- ⏰ Schedule **daily email reminders**
- 📅 **Google Calendar integration** - Sync habits as all-day events
- 💾 Data persistence using local file storage
- 🎨 **Dark Mode support** (See: [THEME_SERVICE.md](THEME_SERVICE.md))
- 🌐 **Multi-language support** with runtime language switching

---

## 🚀 Getting Started

### 🔧 Requirements

- [.NET 6+ SDK](https://dotnet.microsoft.com/en-us/download)
- A Gmail account with [App Passwords enabled](https://support.google.com/accounts/answer/185833) (for email reminders)
- Google account for Calendar integration (optional)

---

## 📥 Installation

1. **Clone this repository**
```bash
git clone https://github.com/Leorev01/Habits_Builder.git
cd habit-tracker-app
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Build the application**
```bash
dotnet build
```

---

## ⚙️ Configuration

### 📧 Email Setup (Optional)

1. **Create a `.env` file** in the project root:
```env
GOOGLE_APP_EMAIL=your-email@gmail.com
GOOGLE_APP_PASSWORD=your-app-password
```

2. **Enable App Passwords** in your Gmail account:
   - Go to [Google Account Settings](https://myaccount.google.com/)
   - Security → 2-Step Verification → App passwords
   - Generate a new app password for "Habit Tracker"

### 📅 Google Calendar Setup (Optional)

#### Quick Setup with Mock File

1. **Create a mock credentials.json file** in your project root:
```json
{
  "installed": {
    "client_id": "YOUR_CLIENT_ID_HERE.apps.googleusercontent.com",
    "project_id": "your-project-id",
    "auth_uri": "https://accounts.google.com/o/oauth2/auth",
    "token_uri": "https://oauth2.googleapis.com/token",
    "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
    "client_secret": "YOUR_CLIENT_SECRET_HERE",
    "redirect_uris": ["http://localhost"]
  }
}
```

2. **Replace with your actual credentials** (see detailed setup below)

#### Detailed Google Cloud Console Setup

1. **Go to Google Cloud Console**
   - Visit [Google Cloud Console](https://console.cloud.google.com/)
   - Sign in with your Google account

2. **Create a new project**
   - Click "New Project"
   - Name it "Habit Tracker"
   - Click "Create"

3. **Enable Google Calendar API**
   - Go to APIs & Services → Library
   - Search for "Google Calendar API"
   - Click "Enable"

4. **Create OAuth 2.0 credentials**
   - Go to APIs & Services → Credentials
   - Click "Create Credentials" → "OAuth client ID"
   - Configure OAuth consent screen (External user type)
   - Fill required fields:
     - App name: "Habit Tracker"
     - User support email: your email
     - Add your email to test users
   - Choose "Desktop application" as application type
   - Name it "Habit Tracker Desktop"
   - Click "Create"

5. **Download and replace credentials**
   - Click the download button (⬇️) next to your OAuth client
   - Replace the mock `credentials.json` file with the downloaded file
   - **OR** copy the values from the downloaded file to your mock file:
     - `client_id`: Replace `YOUR_CLIENT_ID_HERE`
     - `client_secret`: Replace `YOUR_CLIENT_SECRET_HERE`
     - `project_id`: Replace `your-project-id`

6. **First-time authentication**
   - When you first use the calendar sync feature, a browser window will open
   - Sign in to your Google account
   - Grant permission to access your calendar
   - The app will save your authentication tokens for future use

---

## 🏃‍♂️ Running the Application

```bash
dotnet run
```

---

## 📖 Usage

### Main Menu Options

1. **Add Habit** - Create a new daily habit
2. **View Habits** - See all habits with current streaks
3. **Mark Complete** - Mark a habit as completed for today
4. **View History** - See completion history for a specific habit
5. **Delete Habit** - Remove a habit permanently
6. **Set Email Reminders** - Schedule daily email notifications
7. **Change Language** - Switch between supported languages
8. **Toggle Theme** - Switch between light and dark mode
9. **Sync to Google Calendar** - Create calendar events for your habits
10. **Exit** - Save data and quit

### 📅 Calendar Integration

- **Sync Today**: Press Enter when prompted for date
- **Sync Specific Date**: Enter date in format `yyyy-mm-dd` (e.g., `2025-01-07`)
- **All-Day Events**: Habits appear as all-day events in your Google Calendar
- **Smart Reminders**: Automatic popup and email reminders
- **Duplicate Prevention**: Won't create duplicate events for the same habit/date
- **Color Coding**: Habit events appear in green for easy identification

### 🔒 Security Notes

- Your `credentials.json` file contains sensitive information
- Never share this file publicly
- The file is automatically excluded from version control via `.gitignore`
- Authentication tokens are stored locally in `token.json`

---

## 🗂️ Project Structure

```
HabitTrackerApp/
├── Classes/
│   ├── Habit.cs                    # Core habit model
│   ├── Storage.cs                  # Data persistence
│   ├── ReminderService.cs          # Email reminders
│   └── Services/
│       ├── GoogleCalendarService.cs # Calendar integration
│       ├── LocalizationService.cs  # Multi-language support
│       ├── LanguageService.cs      # Language management
│       └── ThemeService.cs         # Theme switching
├── Resources/                      # JSON Localization files
│   ├── en.json                    # English (default)
│   ├── it.json                    # Italian
│   ├── uk.json                    # Ukranian
│   ├── fr.json                    # French
|   └── ru.json                    # Russian
├── credentials.json               # Google API credentials (not in repo)
├── .env                          # Email configuration (not in repo)
├── habits.json                   # Habit data storage
├── token.json                    # Google auth tokens (auto-generated)
└── Program.cs                    # Main application entry point
```

---

## 🌍 Localization

This project supports multiple languages with dynamic runtime switching:

- 🇺🇸 English (default)
- 🇮🇹 Italian
- 🇪🇸 Spanish
- 🇫🇷 French

📝 To add a new language, see [Adding a new language guide](LOCALIZATION_GUIDE.md).

---

## 🎨 Themes

- **Light Mode**: ☀️ Default bright theme
- **Dark Mode**: 🌙 Easy on the eyes for low-light usage
- **Dynamic Switching**: Toggle themes at runtime
- **Persistent Settings**: Your theme preference is saved

See [THEME_SERVICE.md](THEME_SERVICE.md) for more details.

---

## 📝 Contributing

Please read our [contribution guidelines](CONTRIBUTING.md) to learn how to propose changes and report issues.

---

## 🐛 Troubleshooting

### Common Issues

**Calendar sync fails with "Invalid time zone"**
- Ensure you're using the latest version of the app
- Check your internet connection

**Email reminders not working**
- Verify your `.env` file has correct Gmail credentials
- Ensure App Passwords are enabled in Gmail

**"OAuth client was not found" error**
- Download fresh `credentials.json` from Google Cloud Console
- Ensure Google Calendar API is enabled in your project
- Make sure you replaced the mock values with your actual credentials

**"Access denied" error**
- Add your email as a test user in OAuth consent screen
- Or publish your app (not recommended for personal use)

**Mock credentials.json not working**
- Replace `YOUR_CLIENT_ID_HERE` and `YOUR_CLIENT_SECRET_HERE` with actual values
- Ensure the project ID matches your Google Cloud project

### Support

If you encounter issues:
1. Check the troubleshooting section above
2. Review the [Issues](https://github.com/Leorev01/Habits_Builder/issues) page
3. Create a new issue with detailed error information

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- Google Calendar API for seamless calendar integration
- .NET Community for excellent documentation and support
- Contributors who helped improve this application

---

**Happy habit building! 🎯**
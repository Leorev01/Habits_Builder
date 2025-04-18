using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HabitTrackerApp.Classes
{
    public class ReminderService
    {

        private Timer _timer;

        public void SendReminder(List<Habit> habits, TimeSpan time)
        {
            var now = DateTime.Now;
            var nextRun = DateTime.Today.Add(time);
            if (nextRun < now)
                nextRun = nextRun.AddDays(1);

            TimeSpan timeUntilReminder = nextRun - now;

            Console.WriteLine($"⏰ Timer will trigger in: {timeUntilReminder.TotalMinutes:F2} minutes at {nextRun}");

            _timer = new Timer(state =>
            {
                Console.WriteLine($"📨 Timer triggered at {DateTime.Now:T}. Attempting to send email...");

                try
                {
                    var email = Environment.GetEnvironmentVariable("GOOGLE_APP_EMAIL");
                    var password = Environment.GetEnvironmentVariable("GOOGLE_APP_PASSWORD");

                    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    {
                        Console.WriteLine("❌ Missing GOOGLE_APP_EMAIL or GOOGLE_APP_PASSWORD environment variables.");
                        return;
                    }

                    var smtp = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential(email, password),
                        EnableSsl = true
                    };

                    var mail = new MailMessage();
                    mail.From = new MailAddress(email);
                    mail.To.Add(email); // You can change this to any recipient
                    mail.Subject = "Daily Habit Reminder";

                    var body = new StringBuilder("📌 Your habits for today:\n\n");
                    foreach (var habit in habits)
                    {
                        body.AppendLine($"- {habit.Name}");
                    }
                    mail.Body = body.ToString();

                    smtp.Send(mail);
                    Console.WriteLine("✅ Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Email failed: {ex.Message}");
                }

            }, null, timeUntilReminder, TimeSpan.FromDays(1));
        }
    }
}
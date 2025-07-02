using HabitTrackerApp.Classes.Services;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HabitTrackerApp.Classes
{
    public class ReminderService
    {
        private CancellationTokenSource _cts = new();
        private bool _hasStarted = false;

        public void SendReminders(List<Habit> habits, TimeSpan time)
        {
            Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    var now = DateTime.Now;
                    var nextRun = DateTime.Today.Add(time);

                    // if it's already past the scheduled time today, wait until tomorrow
                    if (nextRun < now)
                        nextRun = nextRun.AddDays(1);

                    var delay = nextRun - now;
                    // if delay has negative value, make it 0
                    if (delay < TimeSpan.Zero)
                        delay = TimeSpan.Zero;

                    Console.WriteLine(string.Format(LocalizationService.GetString("ReminderWaiting"), delay.TotalMinutes, nextRun));

                    try
                    {
                        await Task.Delay(delay, _cts.Token);
                        await SendEmailAsync(habits);
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine(LocalizationService.GetString("ReminderCancelled"));
                        break;
                    }

                    // wait 24 hours for next cycle
                    try
                    {
                        await Task.Delay(TimeSpan.FromDays(1), _cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine(LocalizationService.GetString("ReminderCancelledAfterSend"));
                        break;
                    }
                }
            });
        }

        public void StopReminders()
        {
            _cts.Cancel();
        }

        // making the method overridable for testing (not adding interfaces as don't want to change the structure)
        protected virtual async Task SendEmailAsync(List<Habit> habits)
        {
            var email = Environment.GetEnvironmentVariable("GOOGLE_APP_EMAIL");
            var password = Environment.GetEnvironmentVariable("GOOGLE_APP_PASSWORD");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine(LocalizationService.GetString("MissingEmailCredentials"));
                return;
            }

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true
            };

            var body = new StringBuilder(LocalizationService.GetString("EmailBodyHeader"));
            foreach (var habit in habits)
            {
                body.AppendLine(string.Format(LocalizationService.GetString("EmailBodyHabitLine"), habit.Name));
            }

            var mail = new MailMessage(email, email)
            {
                Subject = LocalizationService.GetString("EmailSubject"),
                Body = body.ToString()
            };

            await smtp.SendMailAsync(mail);
            Console.WriteLine(string.Format(LocalizationService.GetString("EmailSent"), DateTime.Now));
        }
    }
}

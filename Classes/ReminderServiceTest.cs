using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HabitTrackerApp.Classes
{
  public class ReminderServiceTests
  {
    private class TestReminderService : ReminderService
    {
      public bool EmailSent { get; private set; } = false;
      public string EmailContent { get; private set; } = "";

      protected override Task SendEmailAsync(List<Habit> habits)
      {
        EmailSent = true;
        EmailContent = string.Join(",", habits.ConvertAll(h => h.Name));
        return Task.CompletedTask;
      }
    }

    [Fact]
    public void ShouldScheduleReminderWithoutError()
    {
      var service = new TestReminderService();
      var habits = new List<Habit> { new Habit("TestHabit") };
      var time = DateTime.Now.AddSeconds(1).TimeOfDay;

      var exception = Record.Exception(() => service.SendReminders(habits, time));

      Assert.Null(exception);
    }

    [Fact]
    public async Task ShouldTriggerEmailSend()
    {
      var service = new TestReminderService();
      var habits = new List<Habit> { new Habit("Exercise") };
      var time = DateTime.Now.AddMilliseconds(100).TimeOfDay;

      service.SendReminders(habits, time);
      await Task.Delay(300);

      Assert.True(service.EmailSent);
      Assert.Contains("Exercise", service.EmailContent);
    }

    [Fact]
    public async Task StartReminders_ShouldIncludeAllHabitsInEmail()
    {
      var service = new TestReminderService();
      var habits = new List<Habit> {
    new Habit("Exercise"),
    new Habit("Run"),
    new Habit("Eat")
  };
      var reminderTime = DateTime.Now.AddMilliseconds(100).TimeOfDay;

      service.SendReminders(habits, reminderTime);
      await Task.Delay(300);

      Assert.True(service.EmailSent);
      Assert.Contains("Exercise", service.EmailContent);
      Assert.Contains("Run", service.EmailContent);
      Assert.Contains("Eat", service.EmailContent);
    }
  }
}
#if __ANDROID__
using System;
using Android.Content;
using Common.Core;

[assembly: Xamarin.Forms.Dependency(typeof(CalendarEvent))]
namespace Common.Core
{
    public class CalendarEvent : ICalendarEvent
    {

        public void CreateCalendarEvent(CalendarEventModel calEvent)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionInsert);
                intent.SetData(Android.Provider.CalendarContract.Events.ContentUri);
                intent.PutExtra(Android.Provider.CalendarContract.ExtraEventBeginTime, CurrentTimeMillis(calEvent.StartTime));
                intent.PutExtra(Android.Provider.CalendarContract.EventsColumns.AllDay, false);
                intent.PutExtra(Android.Provider.CalendarContract.EventsColumns.HasAlarm, calEvent.HasReminder);
                intent.PutExtra(Android.Provider.CalendarContract.EventsColumns.EventLocation, calEvent.Location);
                intent.PutExtra(Android.Provider.CalendarContract.EventsColumns.Description, calEvent.Description);
                intent.PutExtra(Android.Provider.CalendarContract.ExtraEventEndTime, CurrentTimeMillis(calEvent.EndTime));
                intent.PutExtra(Android.Provider.CalendarContract.EventsColumns.Title, calEvent.Title);
                Xamarin.Forms.Forms.Context.StartActivity(intent);
            }
            catch (Exception ex)
            {

            }

        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public long CurrentTimeMillis(DateTime date)
        {
            return (long)(date.ToUniversalTime() - Jan1st1970).TotalMilliseconds;
        }

    }
}
#endif


using System;
using System.Threading.Tasks;

namespace Common.Core
{
    public class CalendarEventModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool HasReminder { get; set; }
    }
    public interface ICalendarEvent
    {
        void CreateCalendarEvent(CalendarEventModel calEvent);
    }
}


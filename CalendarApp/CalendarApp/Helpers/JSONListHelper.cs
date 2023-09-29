using CalendarApp.Models;

namespace CalendarApp.Helpers
{
    public class JSONListHelper
    {
        public static string GetEventListJSONString(List<Event> events)
        {
            var eventList = new List<CalendarEventModel>();
            foreach (var _event in events)
            {
                var _locationName = _event.Location.Name;
                eventList.Add(new CalendarEventModel
                {
                    id = _event.Id,
                    title = _event.Name,
                    start = _event.StartTime, 
                    end = _event.EndTime,
                    description = _event.Description,
                    resourceId = _event.LocationId,
                    allDay = _event.isFullDay,
					locationName = _locationName,

				});
            }
            return System.Text.Json.JsonSerializer.Serialize(eventList);
        }

        public static string GetResourceListJSONString(List<Location> locations)
        {
            var resouseList = new List<CalendarResourceModel>();
            foreach (var _loc in locations)
            {
                resouseList.Add(new CalendarResourceModel
                {
                    id = _loc.Id,
                    title = _loc.Name
                });
            }
            return System.Text.Json.JsonSerializer.Serialize(resouseList);
        }
    }

    public class CalendarEventModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string description { get; set; }
        public int resourceId { get; set; }
        public bool allDay { get; set; }
        public string color { get; set; }
        public string locationName { get; set; }
    }

    public class CalendarResourceModel
    {
        public int id { get; set; }
        public string title { get; set; }
    }
}

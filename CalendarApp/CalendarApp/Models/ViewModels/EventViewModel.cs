using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.Xml;
using System.Security.Policy;

namespace CalendarApp.Models.ViewModels
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public List<SelectListItem> Locations = new List<SelectListItem>();
        public string LocationName { get; set; }    

        public EventViewModel()
        {
            
        }

        public EventViewModel(Event _event, List<Location> locations)
        {
            Event = _event;
            LocationName = _event.Location.Name;
            foreach (var location in locations)
            {
                Locations.Add(new SelectListItem { Text = location.Name, });
            }
        }

        public EventViewModel(List<Location> locations)
        {
            foreach (var location in locations)
            {
                Locations.Add(new SelectListItem { Text = location.Name, });
            }
        }
    }
}

using CalendarApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CalendarApp.Data
{
    public class ApplicationUser : IdentityUser
    {

        public virtual ICollection<Event> Events { get; set; }  
    }
}

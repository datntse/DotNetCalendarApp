using CalendarApp.Data;
using System.ComponentModel.DataAnnotations;

namespace CalendarApp.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //Relationship Data with Location entity and User entity
        public virtual Location Location { get; set; }
        public virtual ApplicationUser User { get; set; }   

    }
}

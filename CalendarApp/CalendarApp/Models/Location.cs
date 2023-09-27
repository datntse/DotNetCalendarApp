using System.ComponentModel.DataAnnotations;

namespace CalendarApp.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    


        //Relation with Events
        public virtual ICollection<Event> Events { get; set; }
    }
}

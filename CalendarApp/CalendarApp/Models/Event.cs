using CalendarApp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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

        public bool isFullDay { get; set; } = false;

        //Relationship Data with Location entity and User entity
        [ForeignKey("LocationId")]
        public int LocationId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual Location Location { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Event()
        {
        }

        public Event(IFormCollection form, Location location)
        {
            Name = form["Event.Name"].ToString();
            Description = form["Event.Description"].ToString();
            StartTime = DateTime.Parse(form["Event.StartTime"].ToString());
            EndTime = DateTime.Parse(form["Event.EndTime"].ToString());
            isFullDay = bool.Parse(form["Event.isFullDay"].ToString().Split(',')[0]);
            UserId = form["Event.UserId"].ToString();
			LocationId = location.Id;
            Location = location;
        }

        //public void UpdateEvent(IFormCollection form, Location location)
        //{
        //    Name = form["Event.Name"].ToString();
        //    Description = form["Event.Description"].ToString();
        //    StartTime = DateTime.Parse(form["Event.StartTime"].ToString());
        //    EndTime = DateTime.Parse(form["Event.EndTime"].ToString());
        //    isFullDay = bool.Parse(form["Event.isFullDay"].ToString().Split(',')[0]);
        //    //LocationId = location.Id;
        //    Location = location;
        //}
        public void UpdateEvent(IFormCollection form, Location location)
        {
            Name = form["Name"].ToString();
            Description = form["Description"].ToString();
            StartTime = DateTime.ParseExact(form["StartTime"].ToString(), "dd/MM/yyyy hh:mm tt", null);
			EndTime = DateTime.ParseExact(form["EndTime"].ToString(), "dd/MM/yyyy hh:mm tt", null);
            if(form["isFullDay"].ToString().Length > 0)
            {
                isFullDay = true;
            } else { isFullDay = false; };
            //LocationId = location.Id;
            Location = location;
        }


    }
}

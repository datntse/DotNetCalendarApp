using CalendarApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarApp.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
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

		public Event EventDetail(IFormCollection form, Location location)
		{
			var name = form["Event.Name"].ToString();
			var description = form["Event.Description"].ToString();
			var startTime = DateTime.Parse(form["Event.StartTime"].ToString());
			var endTime = DateTime.Parse(form["Event.EndTime"].ToString());
			var isFullDay = bool.Parse(form["Event.isFullDay"].ToString().Split(',')[0]);
			var userId = form["Event.UserId"].ToString();
			var locationId = location.Id;

			return new Event
			{
				Name = name,
				Description = description,
				StartTime = startTime,
				EndTime = endTime,
				isFullDay = isFullDay,
				UserId = userId,
				LocationId = locationId,
				Location = location,
			};
		}
		public void UpdateEventDetail(IFormCollection form, Location location)
		{
			Name = form["Event.Name"].ToString();
			Description = form["Event.Description"].ToString();
			StartTime = DateTime.Parse(form["Event.StartTime"].ToString());
			EndTime = DateTime.Parse(form["Event.EndTime"].ToString());
			isFullDay = bool.Parse(form["Event.isFullDay"].ToString().Split(',')[0]);
			//LocationId = location.Id;
			Location = location;
		}
		public Event(IFormCollection form, Location location)
		{
			UserId = form["UserId"].ToString();
			Name = form["Name"].ToString();
			Description = form["Description"].ToString();
			//StartTime = DateTime.Parse(form["StartTime"].ToString());
			//EndTime = DateTime.Parse(form["EndTime"].ToString());

			//Fix create time to reminder
		   StartTime = DateTime.ParseExact(form["StartTime"].ToString(), "dd/MM/yyyy hh:mm tt", null);
			EndTime = DateTime.ParseExact(form["EndTime"].ToString(), "dd/MM/yyyy hh:mm tt", null);

			if (form["isFullDay"].ToString().Length > 0)
			{
				isFullDay = true;
			}
			else
			{
				isFullDay = false;
			};
			Location = location;
		}

		public void UpdateEvent(IFormCollection form, Location location)
		{
			Name = form["Name"].ToString();
			Description = form["Description"].ToString();
			StartTime = DateTime.ParseExact(form["StartTime"].ToString(), "dd/MM/yyyy hh:mm tt", null);
			EndTime = DateTime.ParseExact(form["EndTime"].ToString(), "dd/MM/yyyy hh:mm tt", null);
			if (form["isFullDay"].ToString().Length > 0)
			{
				isFullDay = true;
			}
			else
			{
				isFullDay = false;
			};
			//LocationId = location.Id;
			Location = location;
		}


    }
}

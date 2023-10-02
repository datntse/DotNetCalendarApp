using CalendarApp.Models;
using Microsoft.AspNetCore.Components;

namespace CalendarApp.Data
{
    public interface IDAL
    {
        public List<Event> GetEvents();
        public List<Event> GetMyEvents(string userId);
        public Event GetEvent(int id);
        public Task<Status> CreateEvent(IFormCollection form);
        public Task<Status> CreateEventDetails(IFormCollection form);
		public Task<Status> UpdateEvent(IFormCollection form);
        public Task<Status> DeleteEvent(int id);

        public List<Location> GetLocations();
        public Location GetLocation(int id);
        public Task<Status> CreateLocation(Location location);
        public Task<Status> EditLocation(Location location);

        public string getLocationNameById(int id);
    }
    public class DAL : IDAL
    {
        private readonly ApplicationDbContext _context;

        public DAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Event> GetEvents()
        {
            var result = _context.Events.ToList();
			if(result.Count > 0)
            {
                return result;
            } else
            {
                return new List<Event> { };
            }
        }
        public List<Event> GetMyEvents(string userId)
        {
            return _context.Events.Where(x => x.User.Id.Equals(userId)).ToList();
        }

        public Event GetEvent(int id)
        {
            return _context.Events.FirstOrDefault(x => x.Id == id);
        }
        public async Task<Status> CreateEvent(IFormCollection form)
        {
            var status = new Status();
            var locationName = form["Location"].ToString();

            try
            {
                var locationEvent = _context.Locations.FirstOrDefault(x => x.Name.Equals(locationName));
                if (locationEvent != null)
                {
                    var _event = new Event(form, locationEvent);
                    _context.Events.Add(_event);
                    await _context.SaveChangesAsync();
                    status.Code = 1;
                    status.Message = "Create new Event Successfully";
                } 
            }
            catch (Exception)
            {
                status.Code = 0;
                status.Message = "Invalid data in form or parse error";
            }
            return status;
        }

		public async Task<Status> CreateEventDetails(IFormCollection form)
		{
			var status = new Status();
			var locationName = form["Location"].ToString();
			try
			{
				var locationEvent = _context.Locations.FirstOrDefault(x => x.Name.Equals(locationName));
				if (locationEvent != null)
				{
					var _event = new Event();
                    var _eventDetail = _event.EventDetail(form, locationEvent);
                    _context.Events.Add(_eventDetail);
					await _context.SaveChangesAsync();
					status.Code = 1;
					status.Message = "Create new Event Successfully";
				}
			}
			catch (Exception)
			{
				status.Code = 0;
				status.Message = "Invalid data in form or parse error";
			}
			return status;
		}

		public async Task<Status> DeleteEvent(int id)
        {
            var status = new Status();

            var myEvent = _context.Events.Find(id);
            if (myEvent != null)
            {
                _context.Remove(myEvent);
                await _context.SaveChangesAsync();
                status.Code = 1;
                status.Message = "Deleted Event";
            }
            else
            {
                status.Code = 0;
                status.Message = "Deleted Event Failed";
            }
            return status;

        }
        //public async Task<Status> UpdateEvent(IFormCollection form)
        //{
        //    var status = new Status();
        //    var locationName = form["Location"].ToString();
        //    var eventId = int.Parse(form["Event.Id"]);
        //    try
        //    {
        //        var eventSelectd = _context.Events.FirstOrDefault(x => x.Id == eventId);
        //        var location = _context.Locations.FirstOrDefault(x => x.Name.Equals(locationName));
        //        eventSelectd.UpdateEvent(form, location);

        //        _context.Entry(eventSelectd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //        await _context.SaveChangesAsync();
        //        status.Code = 1;
        //        status.Message = "Update Event Successfully";
        //    }
        //    catch (Exception)
        //    {
        //        status.Code = 0;
        //        status.Message = "Invalid data in form or parse error";
        //    }
        //    return status;
        //}
        // 000000000000 test for ajax 
		public async Task<Status> UpdateEvent(IFormCollection form)
		{
			var status = new Status();
			var locationName = form["Location"].ToString();
			var eventId = int.Parse(form["Id"]);
			try
			{
				var eventSelectd = _context.Events.FirstOrDefault(x => x.Id == eventId);
				var location = _context.Locations.FirstOrDefault(x => x.Name.Equals(locationName));
				eventSelectd.UpdateEvent(form, location);

				_context.Entry(eventSelectd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
				await _context.SaveChangesAsync();
				status.Code = 1;
				status.Message = "Update Event Successfully";
			}
			catch (Exception)
			{
				status.Code = 0;
				status.Message = "Invalid data in form or parse error";
			}
			return status;
		}


		public List<Location> GetLocations()
        {
            return _context.Locations.ToList();
        }

        public Location GetLocation(int id)
        {
            return _context.Locations.Find(id);
        }

        public async Task<Status> CreateLocation(Location location)
        {
            var status = new Status();
            try
            {
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                status.Code = 1;
                status.Message = "Create new Location Successfully";
            }
            catch (Exception)
            {
                status.Code = 0;
                status.Message = "Add Location Failed";
            }
            return status;
        }

        public string getLocationNameById(int id)
        {
            return _context.Locations.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
        }

        public Task<Status> EditLocation(Location location)
        {
            return null; 
        }
    }
}

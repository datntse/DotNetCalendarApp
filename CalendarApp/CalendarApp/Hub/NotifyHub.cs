
using CalendarApp.Data;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CalendarApp.Hub
{

	public interface INotifyHub
	{
		void NotifyTaskForUser(int _eventId);
	};
	public class NotifyHub : Microsoft.AspNetCore.SignalR.Hub
	{

		private readonly ApplicationDbContext _context;

		public NotifyHub(ApplicationDbContext context)
		{
			_context = context;
		}


		public override Task OnConnectedAsync()
		{
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			return base.OnDisconnectedAsync(exception);
		}


	}
}

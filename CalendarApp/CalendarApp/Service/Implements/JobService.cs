using CalendarApp.Data;
using CalendarApp.Hub;
using CalendarApp.Models;
using CalendarApp.Service.Abtract;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CalendarApp.Service.Implements
{
	public class JobService : IJobService
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ApplicationDbContext _context;
		private readonly IHubContext<NotifyHub> _notifyHub;

		public JobService(IBackgroundJobClient backgroundJobClient, ApplicationDbContext context,
			IHubContext<NotifyHub> notifyHub)
		{
			_notifyHub = notifyHub;
			_backgroundJobClient = backgroundJobClient;
			_context = context;
		}
		// tao delayed job khi tao 1 event. 
		// lay so luong trong 1 list luu vo bien static. 


		// tao 1 task thi tao hub . => tao cai schelude. 
		// delayed job. 
		// tao ra 1 delayed job. => 5 phut chay 1 lan
		// sau 5 phut => check xem 
		public async Task ReminderTask(Event _event)
		{
			var _eventId = _event.Id;
			var remindTime = _event.StartTime.Subtract(TimeSpan.FromMinutes(30));

			var listJobArguments = _context.Jobs.Where(x => x.Arguments != null && x.ExpireAt == null).ToList();

			// init job for all backgorund job was expired.
			if (listJobArguments.Count() == 0)
			{
				var jobId = _backgroundJobClient.Schedule(() => NotifyForUser(_eventId), remindTime);
			}

			// tim cai job do. 
			// neu khong co job do thi tao. 
			// layu cai jobid ra
			// xong duyet trong cai mang
			// khong co cai id do thi tao
			// co roi thi update.

			Job _currentJob = null;

			foreach (var job in listJobArguments)
			{
				if (job.Arguments.Contains(_eventId.ToString()))
				{
					_currentJob = job;
					break;

				}
			}

			if (_currentJob != null)
			{
				var currentJobId = _currentJob.Id.ToString();
				BackgroundJob.Delete(currentJobId);
				_backgroundJobClient.Schedule(() => NotifyForUser(_eventId), remindTime);
			}
			else
			{
				_backgroundJobClient.Schedule(() => NotifyForUser(_eventId), TimeSpan.FromSeconds(2));
				//_backgroundJobClient.Enqueue(() => Execute());

			}
		}

		public void Execute()
		{
			// Invoke SignalR method
			_notifyHub.Clients.All.SendAsync("client_function_name", "Hello from Hangfire!");
		}

		public async Task RemoveRemindTask(Event _event)
		{
			var _eventId = _event.Id;
			var remindTime = _event.StartTime.Subtract(TimeSpan.FromMinutes(30));

			var listJobArguments = _context.Jobs.Where(x => x.Arguments != null && x.ExpireAt == null).ToList();
			foreach (var job in listJobArguments)
			{
				if (job.Arguments.Contains(_eventId.ToString()))
				{
					// xóa backgorund job
					var currentJobId = job.Id.ToString();
					BackgroundJob.Delete(currentJobId);
					break;
				}
			}
		}

		// signal R goi invod de nhan data thang ham nay tra ve => xong hien len view.

		public void NotifyForUser(int eventId)
		{


			// disable lazy loading.
			_context.ChangeTracker.LazyLoadingEnabled = false;
			var settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};

			var _eventData = _context.Events.Where(x => x.Id == eventId).FirstOrDefault();
			var _location = _context.Locations.Where(x => x.Id == _eventData.LocationId).Select(x => x.Name).FirstOrDefault();	

			if (_eventData != null)
			{
				var jsonObject = JsonConvert.SerializeObject(_eventData, settings);
				_notifyHub.Clients.All.SendAsync("TaskNotifycation", jsonObject, _location);
			}
		}


	}
}

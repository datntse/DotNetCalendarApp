using CalendarApp.Data;
using CalendarApp.Models;
using CalendarApp.Service.Abtract;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace CalendarApp.Service.Implements
{
	public class JobService : IJobService
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ApplicationDbContext _context;

		public JobService(IBackgroundJobClient backgroundJobClient, ApplicationDbContext context)
		{
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
			} else
			{
				_backgroundJobClient.Schedule(() => NotifyForUser(_eventId), remindTime);
			}
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
		public void NotifyForUser(int eventId)
		{
			// singalR
			Console.WriteLine("Create a schedule Job for notify have event");
		}

		
	}
}

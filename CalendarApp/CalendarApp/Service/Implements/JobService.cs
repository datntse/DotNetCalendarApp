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
		private readonly JobStorage _jobStorage;
		private readonly IMonitoringApi _monitoringApi;
		public JobService(IBackgroundJobClient backgroundJobClient, ApplicationDbContext conte, IMonitoringApi monitoringApixt, JobStorage jobStorage,
			IMonitoringApi monitoringApi)
		{
			_backgroundJobClient = backgroundJobClient;
			_jobStorage = jobStorage;
			_monitoringApi = monitoringApi;
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

			Hangfire.JobStorage.Current.GetMonitoringApi();

			var connection = _jobStorage.GetConnection();
		
				var jobId = _backgroundJobClient.Schedule(() => NotifyForUser(_eventId), remindTime);
		}

		public void NotifyForUser(int eventId)
		{
			// singalR
			Console.WriteLine("Create a schedule Job for notify have event");
		}


	}
}

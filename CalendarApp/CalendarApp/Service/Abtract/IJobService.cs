﻿using CalendarApp.Models;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace CalendarApp.Service.Abtract
{
    public interface IJobService
    {
		Task ReminderTask(Event _event);
		Task RemoveRemindTask(Event _event);

		void CollectionGrabage_Events();
	}
}

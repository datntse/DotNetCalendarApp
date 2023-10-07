using CalendarApp.Data;
using CalendarApp.Helpers;
using CalendarApp.Hub;
using CalendarApp.Models;
using CalendarApp.Models.ViewModels;
using CalendarApp.Service.Abtract;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace CalendarApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly IDAL _idal;
		private readonly IJobService _jobService;
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IBackgroundJobClient _backgroundJobClient;

		private readonly IHubContext<NotifyHub> _notifyHub;
		public HomeController(ILogger<HomeController> logger, IDAL idal,
			UserManager<ApplicationUser> userManager,
			IBackgroundJobClient backgroundJobClient,
			IJobService jobService,
			IHubContext<NotifyHub> notifyHub)
		{
			_notifyHub = notifyHub;
			_idal = idal;
			_jobService = jobService;
			_logger = logger;
			_userManager = userManager;
			_backgroundJobClient = backgroundJobClient;
		}

		public IActionResult Index()
		{
			return View();
		}
		[Authorize]
		public async Task<IActionResult> MyCalendar()
		{
			var _user = await _userManager.GetUserAsync(User);
			if (_user != null)
			{
				var userId = await _userManager.GetUserIdAsync(_user);
				ViewData["UserId"] = userId;

				var _eventList = JSONListHelper.GetEventListJSONString(_idal.GetMyEvents(userId));
				var _locationList = JSONListHelper.GetResourceListJSONString(_idal.GetLocations());
				ViewData["EventList"] = _eventList;
				ViewData["ResourceList"] = _locationList;
			}

			return View(new EventViewModel(_idal.GetLocations()));
		}


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
using CalendarApp.Data;
using CalendarApp.Helpers;
using CalendarApp.Models;
using CalendarApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CalendarApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly IDAL _idal;
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(ILogger<HomeController> logger, IDAL idal, UserManager<ApplicationUser> userManager)
		{
			_idal = idal;
			_logger = logger;
			_userManager = userManager;
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


		public async Task<IActionResult> RemindTask(string userId)
		{
			/// 1 tao reminder sau khi tao 1 event.
			/// sau do tao 1 cai count down. chay song song voi chuong trinh. 
			return View();
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
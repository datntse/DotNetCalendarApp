using CalendarApp.Data;
using CalendarApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CalendarApp.Controllers
{
	public class HomeController : Controller
	{
        private readonly IDAL _idal;
        private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, IDAL idal)
		{
			_idal = idal;
			_logger = logger;
		}

		public IActionResult Index()
		{
			var result = _idal.GetEvent(1);
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
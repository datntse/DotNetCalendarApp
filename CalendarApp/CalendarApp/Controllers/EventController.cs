using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalendarApp.Data;
using CalendarApp.Models;
using CalendarApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CalendarApp.Controllers
{
	public class EventController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IDAL _dal;

		public EventController(IDAL idal, UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			_dal = idal;
		}

		// GET: Event
		public async Task<IActionResult> Index()
		{
			List<EventViewModel> listEventVM = new List<EventViewModel>();
			var _userLogin = await _userManager.GetUserAsync(User);
			if (_userLogin != null)
			{
				var userId = await _userManager.GetUserIdAsync(_userLogin);
				var _events = _dal.GetMyEvents(userId);
				foreach (var _event in _events)
				{
					listEventVM.Add(new EventViewModel
					{
						Event = _event,
						LocationName = _dal.getLocationNameById(_event.LocationId)
					}); ;
				}
			}
			return View(listEventVM);
		}

		// GET: Event/Details/5
		public IActionResult Details(int? id)
		{
			if (id == null || _dal.GetEvents == null)
			{
				return NotFound();
			}

			var @event = _dal.GetEvent((int)id);
			if (@event == null)
			{
				return NotFound();
			}

			return View(@event);
		}

		// GET: Event/Create
		public IActionResult Create()
		{
			return View(new EventViewModel(_dal.GetLocations()));
		}

		// POST: Event/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EventViewModel model, IFormCollection form)
		{
			var result = await _dal.CreateEventDetails(form);
			if (result.Code == 1)
			{
				ViewData["Message"] = result.Message;
				return RedirectToAction(nameof(Index));
			}
			else
			{
				ViewData["Message"] = result.Message;
				return View(model);
			}
		}

		// Ajax call controller
		[HttpPost]
		public async Task<JsonResult> EditEvent(IFormCollection form)
		{
			var eventId = int.Parse(form["id"].ToString());
			var status = new Status();
			
			if(eventId != 0)
			{
				status = await _dal.UpdateEvent(form);
			} else
			{
				status = await _dal.CreateEvent(form);
			}
			return new JsonResult(status.Code);
		}

		[HttpPost]
		public async Task<JsonResult> DeleteEvent(int eventId)
		{
			var result = await _dal.DeleteEvent(eventId);
			var status = new Status
			{
				Code = result.Code
			};
			return new JsonResult(status);
		}

		// GET: Event/Edit/5
		public IActionResult Edit(int? id)
		{
			if (id == null || _dal.GetEvents() == null)
			{
				return NotFound();
			}

			var @event = _dal.GetEvent((int)id);

			//get Location with selected event 
			var locationEvent = _dal.GetLocation(@event.LocationId);
			if (locationEvent != null)
			{
				//var eventVM = new EventViewModel
				//{
				//    Event = @event,
				//    LocationName = locationEvent.Name,
				//};
				var eventVM = new EventViewModel(@event, _dal.GetLocations());
				return View(eventVM);
			}
			else
			{
				return NotFound();

			}

		}

		// POST: Event/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EventViewModel model, IFormCollection form)
		{
			if (id != model.Event.Id)
			{
				return NotFound();
			}


			var result = await _dal.UpdateEvent(form);
			if (result.Code == 1)
			{
				TempData["Message"] = result.Message;
				return RedirectToAction(nameof(Index));
			}
			else
			{
				ViewData["Message"] = result.Message;
			}

			return View(model);
		}

		// GET: Event/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var _event = _dal.GetEvent((int)id);
			if (_event == null)
			{
				return NotFound();
			}
			return View(_event);
		}

		// POST: Event/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{

			var @event = await _dal.DeleteEvent(id);
			TempData["Message"] = @event.Message;
			return RedirectToAction(nameof(Index));
		}

	}
}

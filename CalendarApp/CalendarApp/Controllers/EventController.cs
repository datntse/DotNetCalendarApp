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

namespace CalendarApp.Controllers
{
    public class EventController : Controller
    {
        private readonly IDAL _dal;

        public EventController(IDAL idal)
        {
            _dal = idal;
        }

        // GET: Event
        public IActionResult Index()
        {
           
            return View(_dal.GetEvents());
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
            var result = await _dal.CreateEvent(form);
            if (result.Code == 1)
            {
                ViewData["Message"] = result.Message;
                return RedirectToAction(nameof(Index)); ;
            }
            else
            {
                ViewData["Message"] = result.Message;
                return View(model);
            }
        }

        // GET: Event/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _dal.GetEvents() == null)
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
            if (id == null )
            {
                return NotFound();
            }

            var @event = await _dal.DeleteEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
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

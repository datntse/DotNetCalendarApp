using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CalendarApp.Data;
using CalendarApp.Models;

namespace CalendarApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly IDAL _dal;

        public LocationController(IDAL dal)
        {
            _dal = dal;
        }

        // GET: Location
        public IActionResult Index()
        {
            return View(_dal.GetLocations());
        }

        // GET: Location/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = _dal.GetLocation((int)id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Location location)
        {

            var result = await _dal.CreateLocation(location);
            if (result.Code == 1)
            {
                TempData["Message"] = "Add new Location Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["Message"] = "Have Error In Add new Location Check it out";
            }
            return View(location);
        }

        // GET: Location/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Locations == null)
        //    {
        //        return NotFound();
        //    }

        //    var location = await _context.Locations.FindAsync(id);
        //    if (location == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(location);
        //}

        //POST: Location/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Location location)
        //{
        //    if (id != location.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(location);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!LocationExists(location.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(location);
        //}

        //// GET: Location/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Locations == null)
        //    {
        //        return NotFound();
        //    }

        //    var location = await _context.Locations
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (location == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(location);
        //}

        //// POST: Location/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Locations == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Locations'  is null.");
        //    }
        //    var location = await _context.Locations.FindAsync(id);
        //    if (location != null)
        //    {
        //        _context.Locations.Remove(location);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool LocationExists(int id)
        //{
        //  return (_context.Locations?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}

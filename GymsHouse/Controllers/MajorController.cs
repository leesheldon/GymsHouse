using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymsHouse.Data;
using GymsHouse.Models;

namespace GymsHouse.Controllers
{
    public class MajorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MajorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Major
        public async Task<IActionResult> Index()
        {
            return View(await _context.Major.ToListAsync());
        }

        // GET: Major/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Major
                .SingleOrDefaultAsync(m => m.ID == id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // GET: Major/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Major/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] Major major)
        {
            if (ModelState.IsValid)
            {
                var majorExists = _context.Major
                        .Where(p => p.Name.Equals(major.Name))
                        .Count();

                if (majorExists > 0)
                {
                    ViewBag.ErrMsg = "Error: This major already exists.";
                    return View(major);
                }
                
                _context.Add(major);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(major);
        }

        // GET: Major/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Major.SingleOrDefaultAsync(m => m.ID == id);
            if (major == null)
            {
                return NotFound();
            }
            return View(major);
        }

        // POST: Major/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Description")] Major major)
        {
            if (id != major.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var majorExists = _context.Major
                            .Where(p => p.Name.Equals(major.Name) && p.ID != major.ID)
                            .Count();

                    if (majorExists > 0)
                    {
                        ViewBag.ErrMsg = "Error: This major already exists.";
                        return View(major);
                    }

                    _context.Update(major);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MajorExists(major.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(major);
        }

        // GET: Major/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Major
                .SingleOrDefaultAsync(m => m.ID == id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // POST: Major/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var major = await _context.Major.SingleOrDefaultAsync(m => m.ID == id);
            _context.Major.Remove(major);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MajorExists(string id)
        {
            return _context.Major.Any(e => e.ID == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymsHouse.Data;
using GymsHouse.Models;
using GymsHouse.Models.SchedulerViewModels;

namespace GymsHouse.Controllers
{
    public class HolidaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public HolidaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Holidays
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Holidays
                    .Include(h => h.Center)
                    .OrderByDescending(p => p.CenterId)
                    .ThenByDescending(p => p.Holiday);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Holidays/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var holidays = await _context.Holidays
                .Include(h => h.Center)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (holidays == null)
            {
                return NotFound();
            }

            return View(holidays);
        }

        // GET: Holidays/Create
        public IActionResult Create()
        {
            Holidays holidays = new Holidays();
            holidays.Holiday = DateTime.Now;

            HolidaysAndCenterViewModel vm = new HolidaysAndCenterViewModel
            {
                CenterList = _context.Center.ToList(),
                Holidays = holidays
            };

            return View(vm);
        }

        // POST: Holidays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HolidaysAndCenterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = new HolidaysAndCenterViewModel
                {
                    CenterList = _context.Center.ToList(),
                    Holidays = new Holidays()
                };

                return View(vm);
            }

            var holidayExists = _context.Holidays
                        .Where(p => p.Holiday.Day == vm.Holidays.Holiday.Day
                                && p.Holiday.Month == vm.Holidays.Holiday.Month
                                && p.Holiday.Year == vm.Holidays.Holiday.Year
                                && p.CenterId == vm.Holidays.CenterId)
                        .Count();

            if (holidayExists > 0)
            {
                StatusMessage = "Error: This record has been already existed.";

                vm = new HolidaysAndCenterViewModel
                {
                    CenterList = _context.Center.ToList(),
                    Holidays = new Holidays(),
                    StatusMessage = StatusMessage
                };

                return View(vm);
            }
            else
            {
                _context.Holidays.Add(vm.Holidays);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Holidays/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var holidays = await _context.Holidays.SingleOrDefaultAsync(m => m.ID == id);
            if (holidays == null)
            {
                return NotFound();
            }

            HolidaysAndCenterViewModel vm = new HolidaysAndCenterViewModel
            {
                CenterList = _context.Center.ToList(),
                Holidays = holidays
            };

            return View(vm);
        }

        // POST: Holidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, HolidaysAndCenterViewModel vm)
        {
            if (id != vm.Holidays.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm = new HolidaysAndCenterViewModel
                {
                    CenterList = _context.Center.ToList(),
                    Holidays = new Holidays()
                };

                return View(vm);                
            }

            try
            {
                var holidayFromDB = _context.Holidays.Find(vm.Holidays.ID);
                if (holidayFromDB == null)
                {
                    return NotFound();
                }

                var holidayExists = _context.Holidays
                        .Where(p => p.Holiday.Day == vm.Holidays.Holiday.Day
                                && p.Holiday.Month == vm.Holidays.Holiday.Month
                                && p.Holiday.Year == vm.Holidays.Holiday.Year
                                && p.CenterId == vm.Holidays.CenterId
                                && p.ID != vm.Holidays.ID)
                        .Count();

                if (holidayExists > 0)
                {
                    StatusMessage = "Error: This record has been already existed.";

                    // Error
                    vm = new HolidaysAndCenterViewModel
                    {
                        CenterList = _context.Center.ToList(),
                        Holidays = new Holidays(),
                        StatusMessage = StatusMessage
                    };

                    return View(vm);
                }

                holidayFromDB.Holiday = vm.Holidays.Holiday;
                holidayFromDB.CenterId = vm.Holidays.CenterId;
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                StatusMessage = "Error: " + ex.Message;
                vm = new HolidaysAndCenterViewModel
                {
                    CenterList = _context.Center.ToList(),
                    Holidays = new Holidays(),
                    StatusMessage = StatusMessage
                };

                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Holidays/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var holidays = await _context.Holidays.Include(p => p.Center).SingleOrDefaultAsync(m => m.ID == id);
            if (holidays == null)
            {
                return NotFound();
            }
           
            return View(holidays);
        }

        // POST: Holidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var holidays = await _context.Holidays.SingleOrDefaultAsync(m => m.ID == id);
            if (holidays == null)
            {
                return NotFound();
            }

            _context.Holidays.Remove(holidays);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HolidaysExists(string id)
        {
            return _context.Holidays.Any(e => e.ID == id);
        }
    }
}

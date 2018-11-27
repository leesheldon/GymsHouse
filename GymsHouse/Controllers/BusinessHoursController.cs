using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymsHouse.Data;
using GymsHouse.Models;
using GymsHouse.Models.GymsCentersViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class BusinessHoursController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        public BusinessHoursController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<IActionResult> Index()
        {
            List<BusinessHours> bh = await _db.BusinessHours
                    .Include(p => p.Center)
                    .OrderBy(p => p.Center.Name)
                    .ThenBy(p => p.DisplayOrder)
                    .ToListAsync();

            return View(bh);
        }

        // GET: Business Hours/Create
        public IActionResult Create()
        {
            BusinessHoursAndCenterViewModel vm = new BusinessHoursAndCenterViewModel
            {
                CenterList = _db.Center.ToList(),
                BusinessHours = new BusinessHours()
            };

            return View(vm);
        }

        // POST: Business Hours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BusinessHoursAndCenterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var locationExists = _db.BusinessHours
                        .Where(p => p.DaysOfWeek.ToLower().Equals(vm.BusinessHours.DaysOfWeek.ToLower()) && p.CenterId == vm.BusinessHours.CenterId)
                        .Count();

                if (locationExists > 0)
                {
                    StatusMessage = "Error: This record has been already existed.";
                }
                else
                {
                    _db.BusinessHours.Add(vm.BusinessHours);
                    await _db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            // Error
            vm = new BusinessHoursAndCenterViewModel
            {
                CenterList = _db.Center.ToList(),
                BusinessHours = new BusinessHours(),
                StatusMessage = StatusMessage
            };

            return View(vm);
        }

        // GET Details Business Hours
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var bh = await _db.BusinessHours
                    .Include(p => p.Center)
                    .SingleOrDefaultAsync(p => p.ID == id);

            if (bh == null)
            {
                return NotFound();
            }

            return View(bh);
        }

        // GET: Business Hours/Edit/
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var bh = await _db.BusinessHours
                    .Include(p => p.Center)
                    .SingleOrDefaultAsync(m => m.ID == id);

            if (bh == null)
            {
                return NotFound();
            }

            BusinessHoursAndCenterViewModel vm = new BusinessHoursAndCenterViewModel
            {
                CenterList = _db.Center.ToList(),
                BusinessHours = bh
            };

            return View(vm);
        }

        // POST: Business Hours/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BusinessHoursAndCenterViewModel vm)
        {
            if (vm.BusinessHours.ID != id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm = new BusinessHoursAndCenterViewModel
                {
                    CenterList = _db.Center.ToList()
                };

                return View(vm);
            }

            var bhFromDB = _db.BusinessHours.Find(vm.BusinessHours.ID);
            if (bhFromDB == null)
            {
                return NotFound();
            }

            var bhExists = _db.BusinessHours
                    .Where(p => p.DaysOfWeek.ToLower().Equals(vm.BusinessHours.DaysOfWeek.ToLower())
                        && p.ID != vm.BusinessHours.ID
                        && p.CenterId == vm.BusinessHours.CenterId)
                    .Count();

            if (bhExists > 0)
            {
                StatusMessage = "Error: This record has been already existed.";
                // Error
                vm = new BusinessHoursAndCenterViewModel
                {
                    CenterList = _db.Center.ToList(),
                    BusinessHours = new BusinessHours(),
                    StatusMessage = StatusMessage
                };

                return View(vm);
            }

            bhFromDB.DaysOfWeek = vm.BusinessHours.DaysOfWeek;
            bhFromDB.From = vm.BusinessHours.From;
            bhFromDB.To = vm.BusinessHours.To;
            bhFromDB.CenterId = vm.BusinessHours.CenterId;
            bhFromDB.IsClosed = vm.BusinessHours.IsClosed;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET Delete Business Hours
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var bh = await _db.BusinessHours
                    .Include(p => p.Center)
                    .SingleOrDefaultAsync(p => p.ID == id);

            if (bh == null)
            {
                return NotFound();
            }

            return View(bh);
        }

        // POST: Business Hours/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var bhFromDB = _db.BusinessHours.Find(id);
            if (bhFromDB == null)
            {
                return NotFound();
            }

            _db.BusinessHours.Remove(bhFromDB);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
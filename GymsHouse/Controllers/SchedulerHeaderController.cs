using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymsHouse.Data;
using GymsHouse.Models;
using GymsHouse.Models.SchedulerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class SchedulerHeaderController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string ErrorMessage { get; set; }

        public SchedulerHeaderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.ScheduleHeader
                    .Include(p => p.TrainingClass)
                    .Include(p => p.Location)
                    .Include(p => p.ScheduleDetails)
                    .ToListAsync();

            foreach(ScheduleHeader sh in items)
            {
                if (!string.IsNullOrEmpty(sh.Status))
                {
                    int enumIdx = int.Parse(sh.Status);
                    sh.StatusText = Enum.GetName(typeof(ScheduleHeader.EStatus), enumIdx);
                }
            }

            return View(items);
        }

        // GET: Scheduler/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var sh = await _db.ScheduleHeader
                    .Include(h => h.TrainingClass)
                    .Include(h => h.Instructor)
                    .Include(h => h.Instructor.ApplicationUser)
                    .Include(h => h.Location)
                    .Include(h => h.Location.Center)
                    .SingleOrDefaultAsync(m => m.ID == id);

            if (sh == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(sh.Status))
            {
                int enumIdx = int.Parse(sh.Status);
                sh.StatusText = Enum.GetName(typeof(ScheduleHeader.EStatus), enumIdx);
            }

            // Days of Training
            string daysOfTraining = "";

            if(sh.Monday)
            {
                daysOfTraining = "Monday, ";
            }
            if (sh.Tuesday)
            {
                daysOfTraining = daysOfTraining + "Tuesday, ";
            }
            if (sh.Wednesday)
            {
                daysOfTraining = daysOfTraining + "Wednesday, ";
            }
            if (sh.Thursday)
            {
                daysOfTraining = daysOfTraining + "Thursday, ";
            }
            if (sh.Friday)
            {
                daysOfTraining = daysOfTraining + "Friday, ";
            }
            if (sh.Saturday)
            {
                daysOfTraining = daysOfTraining + "Saturday, ";
            }
            if (sh.Sunday)
            {
                daysOfTraining = daysOfTraining + "Sunday, ";
            }

            if (!string.IsNullOrEmpty(daysOfTraining))
            {
                daysOfTraining = daysOfTraining.Substring(0, daysOfTraining.Length - 2);
                sh.DaysOfTraining = daysOfTraining;
            }

            // Instructor's Name
            sh.Instructor.Name = sh.Instructor.ApplicationUser.FirstName + " " + sh.Instructor.ApplicationUser.LastName;

            return View(sh);
        }

        // GET: Scheduler/Create
        public async Task<IActionResult> Create()
        {
            var instructors = await _db.Instructor.Include(p => p.ApplicationUser).ToListAsync();
            foreach(Instructor instructor in instructors)
            {
                instructor.Name = instructor.ApplicationUser.FirstName + " " + instructor.ApplicationUser.LastName;
            }

            SchedulerFormViewModel vm = new SchedulerFormViewModel
            {
                TrainingClassesList = await _db.TrainingClass.ToListAsync(),
                ScheduleHeader = new ScheduleHeader(),
                LocationsList = GetLocations(),
                CentersList = GetCenters(),
                InstructorsList = instructors
            };

            vm.ScheduleHeader.StartDate = DateTime.Now;
            vm.ScheduleHeader.EndDate = DateTime.Now;

            return View(vm);
        }

        // POST: Scheduler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SchedulerFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var instructors = await _db.Instructor.Include(p => p.ApplicationUser).ToListAsync();
                foreach (Instructor instructor in instructors)
                {
                    instructor.Name = instructor.ApplicationUser.FirstName + " " + instructor.ApplicationUser.LastName;
                }

                vm = new SchedulerFormViewModel
                {
                    TrainingClassesList = await _db.TrainingClass.ToListAsync(),
                    InstructorsList = instructors,
                    LocationsList = GetLocations(),
                    CentersList = GetCenters(),
                    ScheduleHeader = new ScheduleHeader()
                };

                return View(vm);
            }

            var schedulerHeaderExists = _db.ScheduleHeader
                        .Where(p => p.ClassId == vm.ScheduleHeader.ClassId
                                && p.InstructorId == vm.ScheduleHeader.InstructorId
                                && p.LocationId == vm.ScheduleHeader.LocationId
                                && p.StartDate.Day == vm.ScheduleHeader.StartDate.Day
                                && p.StartDate.Month == vm.ScheduleHeader.StartDate.Month
                                && p.StartDate.Year == vm.ScheduleHeader.StartDate.Year)
                        .Count();

            if (schedulerHeaderExists > 0)
            {
                ErrorMessage = "Error: This schedule has been already existed.";

                var instructors = await _db.Instructor.Include(p => p.ApplicationUser).ToListAsync();
                foreach (Instructor instructor in instructors)
                {
                    instructor.Name = instructor.ApplicationUser.FirstName + " " + instructor.ApplicationUser.LastName;
                }

                vm = new SchedulerFormViewModel
                {
                    TrainingClassesList = await _db.TrainingClass.ToListAsync(),
                    InstructorsList = instructors,
                    LocationsList = GetLocations(),
                    CentersList = GetCenters(),
                    ScheduleHeader = new ScheduleHeader(),
                    ErrorMessage = ErrorMessage
                };

                return View(vm);
            }

            _db.ScheduleHeader.Add(vm.ScheduleHeader);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Scheduler/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var sh = await _db.ScheduleHeader.Include(p => p.Location).SingleOrDefaultAsync(m => m.ID == id);
            if (sh == null)
            {
                return NotFound();
            }

            var instructors = await _db.Instructor.Include(p => p.ApplicationUser).ToListAsync();
            foreach (Instructor instructor in instructors)
            {
                instructor.Name = instructor.ApplicationUser.FirstName + " " + instructor.ApplicationUser.LastName;
            }

            SchedulerFormViewModel vm = new SchedulerFormViewModel
            {
                TrainingClassesList = await _db.TrainingClass.ToListAsync(),
                LocationsList = GetLocationsListByCenterId(sh.Location.CenterId),
                CentersList = GetCenters(),
                SelectedCenter = sh.Location.CenterId,
                SelectedLocation = sh.LocationId,
                ScheduleHeader = sh,
                InstructorsList = instructors
            };
            
            return View(vm);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SchedulerFormViewModel vm)
        {
            if (id != vm.ScheduleHeader.ID)
            {
                return NotFound();
            }
                        
            if (!ModelState.IsValid)
            {
                var location = _db.Location.Find(vm.ScheduleHeader.LocationId);
                var instructors = await _db.Instructor.Include(p => p.ApplicationUser).ToListAsync();
                foreach (Instructor instructor in instructors)
                {
                    instructor.Name = instructor.ApplicationUser.FirstName + " " + instructor.ApplicationUser.LastName;
                }

                vm = new SchedulerFormViewModel
                {
                    TrainingClassesList = await _db.TrainingClass.ToListAsync(),
                    LocationsList = GetLocationsListByCenterId(location.CenterId),
                    CentersList = GetCenters(),
                    SelectedCenter = location.CenterId,
                    SelectedLocation = location.ID,
                    ScheduleHeader = vm.ScheduleHeader,
                    InstructorsList = instructors
                };

                return View(vm);
            }

            // Check Schedule Header exists
            var shFromDB = _db.ScheduleHeader.Find(id);
            if (shFromDB == null)
            {
                return NotFound();
            }

            // Check duplicated record
            var shExists = _db.ScheduleHeader
                        .Where(p => p.ClassId == vm.ScheduleHeader.ClassId
                                && p.InstructorId == vm.ScheduleHeader.InstructorId
                                && p.LocationId == vm.ScheduleHeader.LocationId
                                && p.StartDate.Day == vm.ScheduleHeader.StartDate.Day
                                && p.StartDate.Month == vm.ScheduleHeader.StartDate.Month
                                && p.StartDate.Year == vm.ScheduleHeader.StartDate.Year
                                && p.ID != vm.ScheduleHeader.ID)
                        .Count();

            if (shExists > 0)
            {
                ErrorMessage = "Error: This Schedule Header has already been existed.";
                var location = _db.Location.Find(vm.ScheduleHeader.LocationId);
                var instructors = await _db.Instructor.Include(p => p.ApplicationUser).ToListAsync();
                foreach (Instructor instructor in instructors)
                {
                    instructor.Name = instructor.ApplicationUser.FirstName + " " + instructor.ApplicationUser.LastName;
                }

                vm = new SchedulerFormViewModel
                {
                    TrainingClassesList = await _db.TrainingClass.ToListAsync(),
                    LocationsList = GetLocationsListByCenterId(location.CenterId),
                    CentersList = GetCenters(),
                    SelectedCenter = location.CenterId,
                    SelectedLocation = location.ID,
                    ScheduleHeader = vm.ScheduleHeader,
                    InstructorsList = instructors,
                    ErrorMessage = ErrorMessage
                };

                return View(vm);
            }

            shFromDB.ClassId = vm.ScheduleHeader.ClassId;
            shFromDB.InstructorId = vm.ScheduleHeader.InstructorId;
            shFromDB.LocationId = vm.ScheduleHeader.LocationId;
            shFromDB.ClassSize = vm.ScheduleHeader.ClassSize;
            shFromDB.Notes = vm.ScheduleHeader.Notes;
            shFromDB.Status = vm.ScheduleHeader.Status;
            shFromDB.Monday = vm.ScheduleHeader.Monday;
            shFromDB.Tuesday = vm.ScheduleHeader.Tuesday;
            shFromDB.Wednesday = vm.ScheduleHeader.Wednesday;
            shFromDB.Thursday = vm.ScheduleHeader.Thursday;
            shFromDB.Friday = vm.ScheduleHeader.Friday;
            shFromDB.Saturday = vm.ScheduleHeader.Saturday;
            shFromDB.Sunday = vm.ScheduleHeader.Sunday;

            shFromDB.StartDate = vm.ScheduleHeader.StartDate;
            shFromDB.EndDate = vm.ScheduleHeader.EndDate;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Scheduler/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var sh = await _db.ScheduleHeader
                        .Include(h => h.TrainingClass)
                        .Include(h => h.Instructor)
                        .Include(h => h.Instructor.ApplicationUser)
                        .Include(h => h.Location)
                        .Include(h => h.Location.Center)
                        .SingleOrDefaultAsync(m => m.ID == id);

            if (sh == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(sh.Status))
            {
                int enumIdx = int.Parse(sh.Status);
                sh.StatusText = Enum.GetName(typeof(ScheduleHeader.EStatus), enumIdx);
            }

            // Days of Training
            string daysOfTraining = "";

            if (sh.Monday)
            {
                daysOfTraining = "Monday, ";
            }
            if (sh.Tuesday)
            {
                daysOfTraining = daysOfTraining + "Tuesday, ";
            }
            if (sh.Wednesday)
            {
                daysOfTraining = daysOfTraining + "Wednesday, ";
            }
            if (sh.Thursday)
            {
                daysOfTraining = daysOfTraining + "Thursday, ";
            }
            if (sh.Friday)
            {
                daysOfTraining = daysOfTraining + "Friday, ";
            }
            if (sh.Saturday)
            {
                daysOfTraining = daysOfTraining + "Saturday, ";
            }
            if (sh.Sunday)
            {
                daysOfTraining = daysOfTraining + "Sunday, ";
            }

            if (!string.IsNullOrEmpty(daysOfTraining))
            {
                daysOfTraining = daysOfTraining.Substring(0, daysOfTraining.Length - 2);
                sh.DaysOfTraining = daysOfTraining;
            }

            // Instructor's Name
            sh.Instructor.Name = sh.Instructor.ApplicationUser.FirstName + " " + sh.Instructor.ApplicationUser.LastName;

            return View(sh);
        }

        // POST: Scheduler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var sh = await _db.ScheduleHeader
                            .Include(h => h.TrainingClass)
                            .Include(h => h.Instructor)
                            .Include(h => h.Instructor.ApplicationUser)
                            .Include(h => h.Location)
                            .Include(h => h.Location.Center)
                            .SingleOrDefaultAsync(m => m.ID == id);

            if (sh == null)
            {
                return NotFound();
            }

            if (sh.ScheduleDetails != null && sh.ScheduleDetails.Count > 0)
            {
                ViewData["ErrMessage"] = "Error: Please delete Schedule Details first.";
                if (!string.IsNullOrEmpty(sh.Status))
                {
                    int enumIdx = int.Parse(sh.Status);
                    sh.StatusText = Enum.GetName(typeof(ScheduleHeader.EStatus), enumIdx);
                }

                // Days of Training
                string daysOfTraining = "";

                if (sh.Monday)
                {
                    daysOfTraining = "Monday, ";
                }
                if (sh.Tuesday)
                {
                    daysOfTraining = daysOfTraining + "Tuesday, ";
                }
                if (sh.Wednesday)
                {
                    daysOfTraining = daysOfTraining + "Wednesday, ";
                }
                if (sh.Thursday)
                {
                    daysOfTraining = daysOfTraining + "Thursday, ";
                }
                if (sh.Friday)
                {
                    daysOfTraining = daysOfTraining + "Friday, ";
                }
                if (sh.Saturday)
                {
                    daysOfTraining = daysOfTraining + "Saturday, ";
                }
                if (sh.Sunday)
                {
                    daysOfTraining = daysOfTraining + "Sunday, ";
                }

                if (!string.IsNullOrEmpty(daysOfTraining))
                {
                    daysOfTraining = daysOfTraining.Substring(0, daysOfTraining.Length - 2);
                    sh.DaysOfTraining = daysOfTraining;
                }

                // Instructor's Name
                sh.Instructor.Name = sh.Instructor.ApplicationUser.FirstName + " " + sh.Instructor.ApplicationUser.LastName;

                return View(sh);
            }

            _db.ScheduleHeader.Remove(sh);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public JsonResult GetLocationsByCenterId(string centerId)
        {
            if (!string.IsNullOrWhiteSpace(centerId))
            {
                IEnumerable<SelectListItem> locations = _db.Location.AsNoTracking()
                        .OrderBy(n => n.Name)
                        .Where(n => n.CenterId == centerId)
                        .Select(n =>
                           new SelectListItem
                           {
                               Value = n.ID,
                               Text = n.Name
                           }).ToList();
                
                return Json(new SelectList(locations, "Value", "Text"));
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetLocationsListByCenterId(string centerId)
        {
            if (!string.IsNullOrWhiteSpace(centerId))
            {
                List<SelectListItem> locations = _db.Location.AsNoTracking()
                        .Where(n => n.CenterId == centerId)
                        .OrderBy(n => n.Name)
                        .Select(n =>
                            new SelectListItem
                            {
                                Value = n.ID,
                                Text = n.Name
                            })
                        .ToList();

                var locationtip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- select location ---"
                };

                locations.Insert(0, locationtip);

                return new SelectList(locations, "Value", "Text");
            }

            return null;
        }

        public IEnumerable<SelectListItem> GetCenters()
        {
            List<SelectListItem> centers = _db.Center.AsNoTracking()
                        .OrderBy(n => n.Name)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.ID,
                            Text = n.Name
                        }).ToList();

            var centertip = new SelectListItem()
            {
                Value = null,
                Text = "--- select center ---"
            };

            centers.Insert(0, centertip);
            return new SelectList(centers, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetLocations()
        {
            List<SelectListItem> locations = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "--- select location ---"
                }
            };

            return locations;
        }

        [HttpPost, ActionName("CalculateEndDate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateEndDate(string id)
        {
            bool canStop = false;
            int usedHours = 0;
            int usedMinutes = 0;

            var sh = await _db.ScheduleHeader
                            .Include(h => h.TrainingClass)
                            .Include(h => h.Instructor)
                            .Include(h => h.Instructor.ApplicationUser)
                            .Include(h => h.Location)
                            .Include(h => h.Location.Center)
                            .SingleOrDefaultAsync(m => m.ID == id);


            if (sh == null)
            {
                return NotFound();
            }

            int headerDuration = sh.TrainingClass.Duration;
            DateTime runningDate = sh.StartDate;

            List<ScheduleDetails> sdList = await _db.ScheduleDetails.Where(p => p.ScheduleHeaderId == sh.ID).ToListAsync();

            List<Holidays> holidaysList = await _db.Holidays
                                .Where(p => p.CenterId == sh.Location.CenterId
                                            && (p.Holiday.Year == runningDate.Year
                                                || p.Holiday.Year == runningDate.Year + 1))
                                .ToListAsync();

            if (sdList.Count < 1)
            {
                ViewData["ErrMessage"] = "Error: There is no Schedule Detail in this record.";
                return RedirectToAction(nameof(Index));
            }

            if (holidaysList.Count < 1)
            {
                ViewData["ErrMessage"] = "Error: There is no Holidays is set for this year.";
                return RedirectToAction(nameof(Index));
            }

            while (!canStop)
            {
                // Check if running date is holidays
                Holidays holiday = holidaysList.SingleOrDefault(p => p.Holiday.Day == runningDate.Day
                                     && p.Holiday.Month == runningDate.Month
                                     && p.Holiday.Year == runningDate.Year);

                if (holiday != null)
                {
                    runningDate = runningDate.AddDays(1);
                    continue;
                }

                // Check if running date is training date
                ScheduleDetails sd = sdList.SingleOrDefault(p => p.DayOfWeek == runningDate.DayOfWeek.ToString());
                if (sd != null)
                {
                    usedHours = usedHours + sd.Duration_Hours;
                    usedMinutes = usedMinutes + sd.Duration_Minutes;

                    if (usedMinutes > 60)
                    {
                        // If used minutes is > 60, calculate to hours and then add into used hours
                        usedHours = usedHours + (usedMinutes / 60);
                        usedMinutes = usedMinutes % 60;
                    }

                    // Check if used hours is reached to the total hours of this class or not?
                    if (usedHours < headerDuration)
                    {
                        if (headerDuration - usedHours <= 1)
                        {
                            sh.EndDate = runningDate;
                            canStop = true;
                        }
                    }
                    else
                    {
                        sh.EndDate = runningDate;
                        canStop = true;
                    }
                }

                runningDate = runningDate.AddDays(1);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
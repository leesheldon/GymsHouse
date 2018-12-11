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
    public class SchedulerDetailsController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string ErrorMessage { get; set; }

        public SchedulerDetailsController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: SchedulerDetails
        public async Task<IActionResult> Index(string id)
        {            
            #region Schedule Header
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
            #endregion

            #region Schedule Details
            var detailsList = await _db.ScheduleDetails
                            .Include(s => s.ScheduleHeader)
                            .Where(p => p.ScheduleHeaderId == id)
                            .ToListAsync();

            #endregion

            SchedulerDetailsFormViewModel vm = new SchedulerDetailsFormViewModel
            {
                ScheduleDetailsList = detailsList,
                ScheduleHeader = sh
            };

            return View(vm);
        }

        // GET: SchedulerDetails/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var scheduleDetails = await _db.ScheduleDetails
                                        .Include(s => s.ScheduleHeader)
                                        .Include(s => s.ScheduleHeader.TrainingClass)
                                        .SingleOrDefaultAsync(m => m.ID == id);

            if (scheduleDetails == null)
            {
                return NotFound();
            }

            return View(scheduleDetails);
        }

        // GET: SchedulerDetails/Create
        public async Task<IActionResult> Create(string id)
        {
            SchedulerDetailsFormViewModel vm = new SchedulerDetailsFormViewModel();
            vm = await GenerateViewModelForForm(id);

            vm.ScheduleDetails.ScheduleHeaderId = vm.ScheduleHeader.ID;

            return View(vm);
        }

        // POST: SchedulerDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SchedulerDetailsFormViewModel vm)
        {            
            if (!ModelState.IsValid)
            {
                vm = await GenerateViewModelForForm(vm.ScheduleDetails.ScheduleHeaderId);

                return View(vm);
            }

            var schedulerDetailsExists = _db.ScheduleDetails
                       .Where(p => p.ScheduleHeaderId == vm.ScheduleDetails.ScheduleHeaderId
                               && p.DayOfWeek == vm.ScheduleDetails.DayOfWeek
                               && p.From == vm.ScheduleDetails.From
                               && p.To == vm.ScheduleDetails.To)
                       .Count();

            if (schedulerDetailsExists > 0)
            {
                ErrorMessage = "Error: This record has been already existed.";

                vm = await GenerateViewModelForForm(vm.ScheduleDetails.ScheduleHeaderId);

                return View(vm);
            }

            vm.ScheduleDetails.From = vm.SelectedFromText;
            vm.ScheduleDetails.To = vm.SelectedToText;
            vm.ScheduleDetails.DayOfWeek = vm.SelectedDaysText;

            DateTime StartTime = DateTime.Parse(vm.SelectedFromText);
            DateTime EndTime = DateTime.Parse(vm.SelectedToText);
            TimeSpan ts = EndTime - StartTime;
                       
            if (ts.Hours < 1)
            {
                ErrorMessage = "Error: The duration should be equal or greater than 1 hour.";

                vm = await GenerateViewModelForForm(vm.ScheduleDetails.ScheduleHeaderId);

                return View(vm);
            }

            _db.ScheduleDetails.Add(vm.ScheduleDetails);
            await _db.SaveChangesAsync();
                        
            return RedirectToAction(nameof(Index), new { id = vm.ScheduleDetails.ScheduleHeaderId });
        }

        // GET: SchedulerDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var detailItem = await _db.ScheduleDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (detailItem == null)
            {
                return NotFound();
            }

            SchedulerDetailsFormViewModel vm = new SchedulerDetailsFormViewModel();
            vm = await GenerateViewModelForForm(detailItem.ScheduleHeaderId);

            vm.ScheduleDetails.ID = detailItem.ID;
            vm.ScheduleDetails.ScheduleHeaderId = vm.ScheduleHeader.ID;
            vm.SelectedFromText = detailItem.From;
            vm.SelectedToText = detailItem.To;
            vm.SelectedDaysText = detailItem.DayOfWeek;

            return View(vm);
        }

        // POST: SchedulerDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SchedulerDetailsFormViewModel vm)
        {
            if (id != vm.ScheduleDetails.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm = await GenerateViewModelForForm(vm.ScheduleDetails.ScheduleHeaderId);

                return View(vm);
            }

            var detailsFromDB = _db.ScheduleDetails.Find(id);
            if (detailsFromDB == null)
            {
                return NotFound();
            }

            var schedulerDetailsExists = _db.ScheduleDetails
                       .Where(p => p.ScheduleHeaderId == vm.ScheduleDetails.ScheduleHeaderId
                               && p.DayOfWeek == vm.ScheduleDetails.DayOfWeek
                               && p.From == vm.ScheduleDetails.From
                               && p.To == vm.ScheduleDetails.To
                               && p.ID != vm.ScheduleDetails.ID)
                       .Count();

            if (schedulerDetailsExists > 0)
            {
                ErrorMessage = "Error: This record has been already existed.";

                vm = await GenerateViewModelForForm(vm.ScheduleDetails.ScheduleHeaderId);

                return View(vm);
            }

            detailsFromDB.From = vm.SelectedFromText;
            detailsFromDB.To = vm.SelectedToText;
            detailsFromDB.DayOfWeek = vm.SelectedDaysText;
            detailsFromDB.Duration_Hours = vm.ScheduleDetails.Duration_Hours;
            detailsFromDB.Duration_Minutes = vm.ScheduleDetails.Duration_Minutes;

            DateTime StartTime = DateTime.Parse(vm.SelectedFromText);
            DateTime EndTime = DateTime.Parse(vm.SelectedToText);
            TimeSpan ts = EndTime - StartTime;

            if (ts.Hours < 1)
            {
                ErrorMessage = "Error: The duration should be equal or greater than 1 hour.";

                vm = await GenerateViewModelForForm(vm.ScheduleDetails.ScheduleHeaderId);

                return View(vm);
            }
            
            await _db.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index), new { id = vm.ScheduleDetails.ScheduleHeaderId });
        }

        // GET: SchedulerDetails/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var scheduleDetails = await _db.ScheduleDetails
                                        .Include(s => s.ScheduleHeader)
                                        .Include(s => s.ScheduleHeader.TrainingClass)
                                        .SingleOrDefaultAsync(m => m.ID == id);

            if (scheduleDetails == null)
            {
                return NotFound();
            }

            return View(scheduleDetails);
        }

        // POST: SchedulerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var scheduleDetails = await _db.ScheduleDetails
                                        .Include(s => s.ScheduleHeader)
                                        .Include(s => s.ScheduleHeader.TrainingClass)
                                        .SingleOrDefaultAsync(m => m.ID == id);

            if (scheduleDetails == null)
            {
                return NotFound();
            }

            string headerId = scheduleDetails.ScheduleHeaderId;

            _db.ScheduleDetails.Remove(scheduleDetails);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = headerId });
        }

        private bool ScheduleDetailsExists(string id)
        {
            return _db.ScheduleDetails.Any(e => e.ID == id);
        }

        private List<SelectListItem> PopulateTimesList()
        {
            List<SelectListItem> itemsList = new List<SelectListItem>();
            string hh = "";
            string mm = "";
            string pp = "";

            for (int p = 1; p <= 2; p++)
            {
                if (p == 1) pp = "AM";
                else pp = "PM";

                for (int i = 1; i <= 12; i++)
                {
                    int j = 0;
                    while (j < 60)
                    {
                        SelectListItem item = new SelectListItem();
                        if (i < 10) hh = "0" + i.ToString();
                        else hh = i.ToString();

                        if (j < 10) mm = "0" + j.ToString();
                        else mm = j.ToString();

                        item.Text = hh + ":" + mm + " " + pp;
                        item.Value = hh + ":" + mm + " " + pp;

                        itemsList.Add(item);

                        j = j + 10;
                    }
                }
            }

            itemsList.Insert(0, new SelectListItem { Value = "-1", Text = "-- Select time --" });

            return itemsList;
        }

        private List<SelectListItem> PopulateDaysOfTrainingList(ScheduleHeader sh)
        {
            List<SelectListItem> itemsList = new List<SelectListItem>();

            if (sh.Sunday)
            {
                SelectListItem item = new SelectListItem();
                item.Text = "Sunday";
                item.Value = "Sunday";
                itemsList.Add(item);
            }

            if (sh.Monday)
            {
                SelectListItem item = new SelectListItem();
                item.Text = "Monday";
                item.Value = "Monday";
                itemsList.Add(item);
            }

            if (sh.Tuesday)
            {
                SelectListItem item = new SelectListItem();
                item.Text = "Tuesday";
                item.Value = "Tuesday";
                itemsList.Add(item);
            }

            if (sh.Wednesday)
            {
                SelectListItem item = new SelectListItem();
                item.Text = "Wednesday";
                item.Value = "Wednesday";
                itemsList.Add(item);
            }

            if (sh.Thursday)
            {
                SelectListItem item = new SelectListItem();
                item.Text = "Thursday";
                item.Value = "Thursday";
                itemsList.Add(item);
            }

            if (sh.Friday)
            {
                SelectListItem item = new SelectListItem();
                item.Text = "Friday";
                item.Value = "Friday";
                itemsList.Add(item);
            }

            if (sh.Saturday)
            {
                SelectListItem item = new SelectListItem();
                item.Text = "Saturday";
                item.Value = "Saturday";
                itemsList.Add(item);
            }

            itemsList.Insert(0, new SelectListItem { Value = "-1", Text = "-- Select Day --" });

            return itemsList;
        }

        private async Task<SchedulerDetailsFormViewModel> GenerateViewModelForForm(string headerId)
        {
            #region Schedule Header
            var sh = await _db.ScheduleHeader
                            .Include(h => h.TrainingClass)
                            .Include(h => h.Instructor)
                            .Include(h => h.Instructor.ApplicationUser)
                            .Include(h => h.Location)
                            .Include(h => h.Location.Center)
                            .SingleOrDefaultAsync(m => m.ID == headerId);

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
            #endregion

            SchedulerDetailsFormViewModel vm = new SchedulerDetailsFormViewModel
            {
                ScheduleHeader = sh,
                ScheduleDetails = new ScheduleDetails(),
                FromList = PopulateTimesList(),
                ToList = PopulateTimesList(),
                DaysList = PopulateDaysOfTrainingList(sh),
                ErrorMessage = ErrorMessage
            };

            return vm;
        }

        
    }
}

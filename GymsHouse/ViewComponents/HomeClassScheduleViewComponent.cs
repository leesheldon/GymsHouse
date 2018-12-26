using GymsHouse.Data;
using GymsHouse.Models;
using GymsHouse.Models.SchedulerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.ViewComponents
{
    public class HomeClassScheduleViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public HomeClassScheduleViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            DateTime sunday = GetSundayOfWeek(DateTime.Today);
            DateTime monday = sunday.AddDays(1);
            DateTime tuesday = sunday.AddDays(2);
            DateTime wednesday = sunday.AddDays(3);
            DateTime thursday = sunday.AddDays(4);
            DateTime friday = sunday.AddDays(5);
            DateTime saturday = sunday.AddDays(6);           

            List<ScheduleDetails> detailsList = await _db.ScheduleDetails
                                                .Include(p => p.ScheduleHeader)
                                                .Include(p => p.ScheduleHeader.TrainingClass)
                                                .Include(p => p.ScheduleHeader.Instructor)
                                                .Include(p => p.ScheduleHeader.Instructor.ApplicationUser)
                                                .Where(p => p.ScheduleHeader.Status == "2" // 2 --> Started
                                                            && DateTime.Compare(p.ScheduleHeader.EndDate, sunday) >= 0)
                                                .ToListAsync();

            ClassScheduleWeeklyViewModel vm = new ClassScheduleWeeklyViewModel();

            // Sunday
            vm.ScheduleOnSunday = detailsList
                                    .Where(p => p.DayOfWeek == "Sunday")
                                    .Take(8)
                                    .ToList();

            // Monday
            vm.ScheduleOnMonday = detailsList
                                    .Where(p => p.DayOfWeek == "Monday")
                                    .Take(8)
                                    .ToList();

            // Tuesday
            vm.ScheduleOnTuesday = detailsList
                                    .Where(p => p.DayOfWeek == "Tuesday")
                                    .Take(8)
                                    .ToList();

            // Wednesday
            vm.ScheduleOnWednesday = detailsList
                                    .Where(p => p.DayOfWeek == "Wednesday")
                                    .Take(8)
                                    .ToList();

            // Thursday
            vm.ScheduleOnThursday = detailsList
                                    .Where(p => p.DayOfWeek == "Thursday")
                                    .Take(8)
                                    .ToList();

            // Friday
            vm.ScheduleOnFriday = detailsList
                                    .Where(p => p.DayOfWeek == "Friday")
                                    .Take(8)
                                    .ToList();

            // Saturday
            vm.ScheduleOnSaturday = detailsList
                                    .Where(p => p.DayOfWeek == "Saturday")
                                    .Take(8)
                                    .ToList();

            //ClassScheduleWeeklyViewModel vm = new ClassScheduleWeeklyViewModel
            //{
            //    ScheduleOnSunday = await _db.ScheduleDetails
            //                                .Include(p => p.ScheduleHeader)
            //                                .Include(p => p.ScheduleHeader.TrainingClass)
            //                                .Include(p => p.ScheduleHeader.Instructor)
            //                                .Include(p => p.ScheduleHeader.Instructor.ApplicationUser)
            //                                .Where(p => p.DayOfWeek == "Sunday" && DateTime.Compare(p.ScheduleHeader.EndDate, sunday) >= 0)
            //                                .Take(8)
            //                                .ToListAsync(),
            //    ScheduleOnMonday = await _db.ScheduleDetails
            //                                .Include(p => p.ScheduleHeader)
            //                                .Include(p => p.ScheduleHeader.TrainingClass)
            //                                .Include(p => p.ScheduleHeader.Instructor)
            //                                .Include(p => p.ScheduleHeader.Instructor.ApplicationUser)
            //                                .Where(p => p.DayOfWeek == "Monday" && DateTime.Compare(p.ScheduleHeader.EndDate, monday) >= 0)
            //                                .Take(8)
            //                                .ToListAsync(),
            //    ScheduleOnTuesday = await _db.ScheduleDetails
            //                                .Include(p => p.ScheduleHeader)
            //                                .Include(p => p.ScheduleHeader.TrainingClass)
            //                                .Include(p => p.ScheduleHeader.Instructor)
            //                                .Include(p => p.ScheduleHeader.Instructor.ApplicationUser)
            //                                .Where(p => p.DayOfWeek == "Tuesday" && DateTime.Compare(p.ScheduleHeader.EndDate, tuesday) >= 0)
            //                                .Take(8)
            //                                .ToListAsync(),
            //    ScheduleOnWednesday = await _db.ScheduleDetails
            //                                .Include(p => p.ScheduleHeader)
            //                                .Include(p => p.ScheduleHeader.TrainingClass)
            //                                .Include(p => p.ScheduleHeader.Instructor)
            //                                .Include(p => p.ScheduleHeader.Instructor.ApplicationUser)
            //                                .Where(p => p.DayOfWeek == "Wednesday" && DateTime.Compare(p.ScheduleHeader.EndDate, wednesday) >= 0)
            //                                .Take(8)
            //                                .ToListAsync(),
            //};

            return View(vm);
        }


        private DateTime GetSundayOfWeek(DateTime today)
        {
            DateTime returnDate = today;

            if (returnDate.DayOfWeek != DayOfWeek.Sunday)
            {
                while (returnDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    returnDate = returnDate.AddDays(-1);
                }
            }

            return returnDate;
        }

    }
}

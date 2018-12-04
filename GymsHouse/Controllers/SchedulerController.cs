using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymsHouse.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SchedulerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.ScheduleHeader.ToListAsync());
        }


    }
}
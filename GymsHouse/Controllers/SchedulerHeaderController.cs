using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymsHouse.Data;
using GymsHouse.Models.SchedulerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class SchedulerHeaderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SchedulerHeaderController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            return View();
        }


        
        

    }
}
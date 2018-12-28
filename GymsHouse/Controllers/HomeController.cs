using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GymsHouse.Models;
using GymsHouse.Models.TrainingInforViewModels;
using GymsHouse.Data;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<TrainingClass> trainingClassesList = await _db.TrainingClass
                                                                .Take(6)
                                                                .ToListAsync();

            List<Instructor> instructorsList = await _db.Instructor
                                                        .Take(6)
                                                        .ToListAsync();

            TrainingClassesViewModelcs vm = new TrainingClassesViewModelcs();
            vm.TrainingClassesList = trainingClassesList;
            vm.InstructorsList = instructorsList;

            return View(vm);
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

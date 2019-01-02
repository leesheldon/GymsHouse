using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymsHouse.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class TrainingClassesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TrainingClassesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: TrainingClasses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingClass = await _db.TrainingClass
                                            .SingleOrDefaultAsync(m => m.ID == id);

            if (trainingClass == null)
            {
                return NotFound();
            }

            return View(trainingClass);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymsHouse.Data;
using GymsHouse.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using GymsHouse.Extensions;

namespace GymsHouse.Controllers
{
    public class AdminTrainingClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        [TempData]
        public string StatusMessage { get; set; }

        public AdminTrainingClassesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: TrainingClasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingClass.ToListAsync());
        }

        // GET: TrainingClasses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingClass = await _context.TrainingClass
                .SingleOrDefaultAsync(m => m.ID == id);
            if (trainingClass == null)
            {
                return NotFound();
            }

            return View(trainingClass);
        }

        // GET: TrainingClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Price,Duration,IsActive,Picture_1,Picture_2,Picture_3,Picture_4")] TrainingClass trainingClass)
        {
            if (!ModelState.IsValid)
            {
                return View(trainingClass);
            }

            var classExists = _context.TrainingClass
                        .Where(p => p.Name.Equals(trainingClass.Name))
                        .Count();

            if (classExists > 0)
            {
                StatusMessage = "Error: Class Name already exists.";
                return View(trainingClass);
            }

            _context.TrainingClass.Add(trainingClass);
            await _context.SaveChangesAsync();

            // Saving Photos
            var classFromDB = _context.TrainingClass.Find(trainingClass.ID);
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            string classFolder = @"Photos\TrainingClasses";

            #region When user upload Picture 1
            if (files[0] != null && files[0].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(classFolder, classFromDB, files[0], 0);
            }
            else
            {
                // When user does NOT upload an image
                SavingDefaultPhotoPathToDBAndWebRoot(classFolder, classFromDB, 0);
            }
            #endregion

            #region When user upload Picture 2
            if (files[1] != null && files[1].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(classFolder, classFromDB, files[1], 1);
            }
            else
            {
                // When user does NOT upload an image
                SavingDefaultPhotoPathToDBAndWebRoot(classFolder, classFromDB, 1);
            }
            #endregion

            #region When user upload Picture 3
            if (files[2] != null && files[2].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(classFolder, classFromDB, files[2], 2);
            }
            else
            {
                // When user does NOT upload an image
                SavingDefaultPhotoPathToDBAndWebRoot(classFolder, classFromDB, 2);
            }
            #endregion

            #region When user upload Picture 4
            if (files[3] != null && files[3].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(classFolder, classFromDB, files[3], 3);
            }
            else
            {
                // When user does NOT upload an image
                SavingDefaultPhotoPathToDBAndWebRoot(classFolder, classFromDB, 3);
            }
            #endregion

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingClasses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var trainingClass = await _context.TrainingClass.SingleOrDefaultAsync(m => m.ID == id);
            if (trainingClass == null)
            {
                return NotFound();
            }
            return View(trainingClass);
        }

        // POST: TrainingClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Description,Price,Duration,IsActive,Picture_1,Picture_2,Picture_3,Picture_4")] TrainingClass trainingClass)
        {
            if (id != trainingClass.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(trainingClass);
            }

            var classFromDB = _context.TrainingClass.Find(id);
            if (classFromDB == null)
            {
                return NotFound();
            }

            var classExists = _context.TrainingClass
                   .Where(p => p.Name.Equals(trainingClass.Name)
                       && p.ID != id)
                   .Count();

            if (classExists > 0)
            {
                StatusMessage = "Error: Class Name already exists.";
                return View(trainingClass);
            }

            classFromDB.Name = trainingClass.Name;
            classFromDB.Description = trainingClass.Description;
            classFromDB.Price = trainingClass.Price;
            classFromDB.Duration = trainingClass.Duration;
            classFromDB.IsActive = trainingClass.IsActive;

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            string classFolder = @"Photos\TrainingClasses";

            if (!System.IO.Directory.Exists(Path.Combine(webRootPath, classFolder)))
                System.IO.Directory.CreateDirectory(Path.Combine(webRootPath, classFolder));

            #region When user upload Picture 1
            if (files[0] != null && files[0].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(classFolder, classFromDB, files[0], 0);
            }
            #endregion

            #region When user upload Picture 2
            if (files[1] != null && files[1].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(classFolder, classFromDB, files[1], 1);
            }
            #endregion

            #region When user upload Picture 3
            if (files[2] != null && files[2].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(classFolder, classFromDB, files[2], 2);
            }
            #endregion

            #region When user upload Picture 4
            if (files[3] != null && files[3].Length > 0)
            {                
                SavingPhotoPathToDBAndWebRoot(classFolder, classFromDB, files[3], 3);
            }
            #endregion

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingClasses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var trainingClass = await _context.TrainingClass
                .SingleOrDefaultAsync(m => m.ID == id);
            if (trainingClass == null)
            {
                return NotFound();
            }

            return View(trainingClass);
        }

        // POST: TrainingClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var classFromDB = await _context.TrainingClass.SingleOrDefaultAsync(m => m.ID == id);
            if (classFromDB == null)
            {
                return NotFound();
            }

            // Delete Photos
            string webRootPath = _hostingEnvironment.WebRootPath;
            string pathOfOldPic = string.IsNullOrEmpty(classFromDB.Picture_1) ? "" : @"\" + classFromDB.Picture_1.Replace(@"/", @"\");

            if (System.IO.File.Exists(webRootPath + pathOfOldPic))
            {
                System.IO.File.Delete(webRootPath + pathOfOldPic);
            }

            pathOfOldPic = string.IsNullOrEmpty(classFromDB.Picture_2) ? "" : @"\" + classFromDB.Picture_2.Replace(@"/", @"\");
            if (System.IO.File.Exists(webRootPath + pathOfOldPic))
            {
                System.IO.File.Delete(webRootPath + pathOfOldPic);
            }

            pathOfOldPic = string.IsNullOrEmpty(classFromDB.Picture_3) ? "" : @"\" + classFromDB.Picture_3.Replace(@"/", @"\");
            if (System.IO.File.Exists(webRootPath + pathOfOldPic))
            {
                System.IO.File.Delete(webRootPath + pathOfOldPic);
            }

            pathOfOldPic = string.IsNullOrEmpty(classFromDB.Picture_4) ? "" : @"\" + classFromDB.Picture_4.Replace(@"/", @"\");
            if (System.IO.File.Exists(webRootPath + pathOfOldPic))
            {
                System.IO.File.Delete(webRootPath + pathOfOldPic);
            }

            // Delete item
            _context.TrainingClass.Remove(classFromDB);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TrainingClassExists(string id)
        {
            return _context.TrainingClass.Any(e => e.ID == id);
        }

        private void SavingPhotoPathToDBAndWebRoot(string classFolder, TrainingClass classFromDB, IFormFile imageFile, int idx)
        {
            string prevName = "";
            string pathOfOldPic = "";

            if (idx == 0)
            {
                prevName = "pic1_";
                pathOfOldPic = classFromDB.Picture_1;
            }
            else if (idx == 1)
            {
                prevName = "pic2_";
                pathOfOldPic = classFromDB.Picture_2;
            }
            else if (idx == 2)
            {
                prevName = "pic3_";
                pathOfOldPic = classFromDB.Picture_3;
            }
            else
            {
                prevName = "pic4_";
                pathOfOldPic = classFromDB.Picture_4;
            }

            if (!string.IsNullOrEmpty(pathOfOldPic))
            {
                pathOfOldPic = pathOfOldPic.Replace(@"/", @"\");
            }

            string fileName = imageFile.FileName;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string uploadLocation = Path.Combine(webRootPath, classFolder);

            if (!System.IO.Directory.Exists(uploadLocation))
                System.IO.Directory.CreateDirectory(uploadLocation);

            var extensionOfFile = fileName.Substring(fileName.LastIndexOf("."),
                           fileName.Length - fileName.LastIndexOf("."));

            string imageFilePath = classFolder + @"\" + prevName + classFromDB.ID + extensionOfFile;

            uploadLocation = Path.Combine(webRootPath, imageFilePath);

            // Delete Old upload image file
            if (System.IO.File.Exists(webRootPath + @"\" + pathOfOldPic))
            {
                System.IO.File.Delete(webRootPath + @"\" + pathOfOldPic);
            }

            // Delete New upload image file if it is existed
            if (System.IO.File.Exists(uploadLocation))
            {
                System.IO.File.Delete(uploadLocation);
            }

            using (var fileStream = new FileStream(uploadLocation, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            classFolder = classFolder.Replace(@"\", @"/");
            imageFilePath = classFolder + @"/" + prevName + classFromDB.ID + extensionOfFile;
            if (idx == 0)
            {
                classFromDB.Picture_1 = imageFilePath;
            }
            else if (idx == 1)
            {
                classFromDB.Picture_2 = imageFilePath;
            }
            else if (idx == 2)
            {
                classFromDB.Picture_3 = imageFilePath;
            }
            else
            {
                classFromDB.Picture_4 = imageFilePath;
            }

        }

        private void SavingDefaultPhotoPathToDBAndWebRoot(string classFolder, TrainingClass classFromDB, int idx)
        {
            string prevName = "";
            if (idx == 0)
            {
                prevName = "pic1_";
            }
            else if (idx == 1)
            {
                prevName = "pic2_";
            }
            else if (idx == 2)
            {
                prevName = "pic3_";
            }
            else
            {
                prevName = "pic4_";
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            string imageFilePath = Path.Combine(webRootPath, @"Photos\" + SD.DefaultImage);
            string uploadLocation = Path.Combine(webRootPath, classFolder);
            string newImageFilePath = Path.Combine(uploadLocation, prevName + classFromDB.ID + ".png");

            if (!System.IO.Directory.Exists(uploadLocation))
                System.IO.Directory.CreateDirectory(uploadLocation);

            if (!System.IO.File.Exists(newImageFilePath))
            {
                System.IO.File.Copy(imageFilePath, newImageFilePath);
            }

            classFolder = classFolder.Replace(@"\", @"/");
            newImageFilePath = classFolder + @"/" + prevName + classFromDB.ID + ".png";

            if (idx == 0)
            {
                classFromDB.Picture_1 = newImageFilePath;
            }
            else if (idx == 1)
            {
                classFromDB.Picture_2 = newImageFilePath;
            }
            else if (idx == 2)
            {
                classFromDB.Picture_3 = newImageFilePath;
            }
            else
            {
                classFromDB.Picture_4 = newImageFilePath;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GymsHouse.Data;
using GymsHouse.Extensions;
using GymsHouse.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class AdminCenterController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AdminCenterController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Center.ToListAsync());
        }

        // GET: Center/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Centers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Center center)
        {
            if(!ModelState.IsValid)
            {
                return View(center);
            }

            _db.Center.Add(center);
            await _db.SaveChangesAsync();

            // Saving Photos
            var centerFromDB = _db.Center.Find(center.ID);
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            string centersFolder = @"Photos\Centers";
            string uploadCentersFolder = Path.Combine(webRootPath, centersFolder);

            if (!System.IO.Directory.Exists(uploadCentersFolder))
                System.IO.Directory.CreateDirectory(uploadCentersFolder);

            #region When user upload Picture 1
            if (files[0] != null && files[0].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, files[0], 0);
            }
            else
            {
                // When user does NOT upload an image
                SavingDefaultPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, 0);
            }
            #endregion

            #region When user upload Picture 2
            if (files[1] != null && files[1].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, files[1], 1);
            }
            else
            {
                // When user does NOT upload an image
                SavingDefaultPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, 1);
            }
            #endregion

            #region When user upload Picture 3
            if (files[2] != null && files[2].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, files[2], 2);
            }
            else
            {
                // When user does NOT upload an image
                SavingDefaultPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, 2);
            }
            #endregion

            #region When user upload Picture 4
            if (files[3] != null && files[3].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, files[3], 3);
            }
            else
            {
                // When user does NOT upload an image
                SavingDefaultPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, 3);
            }
            #endregion
                        
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET Details Center
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var center = await _db.Center                    
                    .SingleOrDefaultAsync(p => p.ID == id);

            if (center == null)
            {
                return NotFound();
            }

            return View(center);
        }

        // GET: Center/Edit/
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var center = await _db.Center.SingleOrDefaultAsync(m => m.ID == id);
            if (center == null)
            {
                return NotFound();
            }

            return View(center);
        }

        // POST: Center/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Center center)
        {
            if (center.ID != id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(center);
            }

            var centerFromDB = _db.Center.Find(center.ID);
            if (centerFromDB == null)
            {
                return NotFound();
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            string centersFolder = @"Photos\Centers";
            string uploadCentersFolder = Path.Combine(webRootPath, centersFolder);

            if (!center.Name.Equals(centerFromDB.Name))
            {
                // Center Name has been changed                 
                // --> Move files from Old Photos folder to New Photos folder
                if (System.IO.Directory.Exists(uploadCentersFolder + @"\" + centerFromDB.Name))
                {
                    System.IO.Directory.Move(uploadCentersFolder + @"\" + centerFromDB.Name, uploadCentersFolder + @"\" + center.Name);                    
                }
            }

            centerFromDB.Name = center.Name;
            centerFromDB.Address = center.Address;
            centerFromDB.PhoneNumber = center.PhoneNumber;
            centerFromDB.Email = center.Email;

            #region When user upload Picture 1
            if (files[0] != null && files[0].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, files[0], 0);
            }
            
            #endregion

            #region When user upload Picture 2
            if (files[1] != null && files[1].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, files[1], 1);
            }
            
            #endregion

            #region When user upload Picture 3
            if (files[2] != null && files[2].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, files[2], 2);
            }
            #endregion

            #region When user upload Picture 4
            if (files[3] != null && files[3].Length > 0)
            {
                SavingPhotoPathToDBAndWebRoot(centersFolder, centerFromDB, files[3], 3);
            }
            
            #endregion
                       
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET Delete Center
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var center = await _db.Center
                    .SingleOrDefaultAsync(p => p.ID == id);

            if (center == null)
            {
                return NotFound();
            }

            return View(center);
        }

        // POST: Center/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var centerFromDB = _db.Center.Find(id);
            if (centerFromDB == null)
            {
                return NotFound();
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            string centersFolder = @"Photos\Centers";
            string uploadLocation = Path.Combine(webRootPath, centersFolder + @"\" + centerFromDB.Name);

            if (System.IO.Directory.Exists(uploadLocation))
            {
                System.IO.Directory.Delete(uploadLocation, true);
            }

            _db.Center.Remove(centerFromDB);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void SavingPhotoPathToDBAndWebRoot(string centersFolder, Center centerFromDB, IFormFile imageFile, int idx)
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

            string fileName = imageFile.FileName;
            string webRootPath = _hostingEnvironment.WebRootPath;    
            string uploadLocation = Path.Combine(webRootPath, centersFolder + @"\" + centerFromDB.Name);

            if (!System.IO.Directory.Exists(uploadLocation))
                System.IO.Directory.CreateDirectory(uploadLocation);

            var extensionOfFile = fileName.Substring(fileName.LastIndexOf("."),
                           fileName.Length - fileName.LastIndexOf("."));

            string imageFilePath = centersFolder + @"\" + centerFromDB.Name + @"\" + prevName + centerFromDB.ID + extensionOfFile;

            uploadLocation = Path.Combine(webRootPath, imageFilePath);

            if (System.IO.File.Exists(uploadLocation))
            {
                System.IO.File.Delete(uploadLocation);
            }

            using (var fileStream = new FileStream(uploadLocation, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            imageFilePath = @"\" + imageFilePath;
            if (idx == 0)
            {
                centerFromDB.Picture_1 = imageFilePath;
            }
            else if (idx == 1)
            {
                centerFromDB.Picture_2 = imageFilePath;
            }
            else if (idx == 2)
            {
                centerFromDB.Picture_3 = imageFilePath;
            }
            else
            {
                centerFromDB.Picture_4 = imageFilePath;
            }                    
        }

        private void SavingDefaultPhotoPathToDBAndWebRoot(string centersFolder, Center centerFromDB, int idx)
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
            string uploadCentersFolder = Path.Combine(webRootPath, centersFolder);

            string imageFilePath = Path.Combine(webRootPath, @"Photos\" + SD.DefaultImage);
            string uploadLocation = Path.Combine(uploadCentersFolder, centerFromDB.Name);
            string newImageFilePath = Path.Combine(uploadLocation, prevName + centerFromDB.ID + ".png");

            if (!System.IO.Directory.Exists(uploadLocation))
                System.IO.Directory.CreateDirectory(uploadLocation);

            if (!System.IO.File.Exists(newImageFilePath))
            {
                System.IO.File.Copy(imageFilePath, newImageFilePath);
            }

            newImageFilePath = @"\" + centersFolder + @"\" + centerFromDB.Name + @"\" + prevName + centerFromDB.ID + ".png";

            if (idx == 0)
            {
                centerFromDB.Picture_1 = newImageFilePath;
            }
            else if (idx == 1)
            {
                centerFromDB.Picture_2 = newImageFilePath;
            }
            else if (idx == 2)
            {
                centerFromDB.Picture_3 = newImageFilePath;
            }
            else
            {
                centerFromDB.Picture_4 = newImageFilePath;
            }            
        }



    }
}
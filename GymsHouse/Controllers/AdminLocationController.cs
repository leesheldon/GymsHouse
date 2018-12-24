using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GymsHouse.Data;
using GymsHouse.Extensions;
using GymsHouse.Models;
using GymsHouse.Models.GymsCentersViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class AdminLocationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        [TempData]
        public string StatusMessage { get; set; }

        public AdminLocationController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Location.Include(p => p.Center).ToListAsync());
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            LocationAndCenterViewModel vm = new LocationAndCenterViewModel
            {
                CenterList = _db.Center.ToList(),
                Location = new Location(),
                LocationList = _db.Location
                                    .OrderBy(p => p.Name)
                                    .Select(p => p.Name)
                                    .Distinct()
                                    .ToList()
            };

            return View(vm);
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationAndCenterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var locationExists = _db.Location
                    .Where(p => p.Name.Equals(vm.Location.Name) && p.CenterId == vm.Location.CenterId)
                    .Count();

                if (locationExists > 0)
                {
                    StatusMessage = "Error: Location Name already exists.";
                }
                else
                {
                    _db.Location.Add(vm.Location);
                    await _db.SaveChangesAsync();

                    // Saving Photos
                    var locationFromDB = _db.Location.Find(vm.Location.ID);
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    string locationsFolder = @"Photos\Locations";                        

                    #region When user upload Picture 1
                    if (files[0] != null && files[0].Length > 0)
                    {
                        SavingPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, files[0], 0);
                    }
                    else
                    {
                        // When user does NOT upload an image
                        SavingDefaultPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, 0);
                    }
                    #endregion

                    #region When user upload Picture 2
                    if (files[1] != null && files[1].Length > 0)
                    {
                        SavingPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, files[1], 1);
                    }
                    else
                    {
                        // When user does NOT upload an image
                        SavingDefaultPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, 1);
                    }
                    #endregion

                    #region When user upload Picture 3
                    if (files[2] != null && files[2].Length > 0)
                    {
                        SavingPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, files[2], 2);
                    }
                    else
                    {
                        // When user does NOT upload an image
                        SavingDefaultPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, 2);
                    }
                    #endregion

                    #region When user upload Picture 4
                    if (files[3] != null && files[3].Length > 0)
                    {
                        SavingPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, files[3], 3);
                    }
                    else
                    {
                        // When user does NOT upload an image
                        SavingDefaultPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, 3);
                    }
                    #endregion

                    await _db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }                
            }

            // Error
            vm = new LocationAndCenterViewModel
            {
                CenterList = _db.Center.ToList(),
                Location = new Location(),
                LocationList = _db.Location
                                    .OrderBy(p => p.Name)
                                    .Select(p => p.Name)
                                    .Distinct()
                                    .ToList(),
                StatusMessage = StatusMessage
            };

            return View(vm);
        }

        // GET Details Location
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var location = await _db.Location
                    .Include(p => p.Center)
                    .SingleOrDefaultAsync(p => p.ID == id);

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Location/Edit/
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var location = await _db.Location
                    .Include(p => p.Center)
                    .SingleOrDefaultAsync(m => m.ID == id);

            if (location == null)
            {
                return NotFound();
            }

            LocationAndCenterViewModel vm = new LocationAndCenterViewModel
            {
                CenterList = _db.Center.ToList(),
                Location = location,
                LocationList = _db.Location
                                    .OrderBy(p => p.Name)
                                    .Select(p => p.Name)
                                    .Distinct()
                                    .ToList()
            };

            return View(vm);
        }

        // POST: Location/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, LocationAndCenterViewModel vm)
        {
            if (vm.Location.ID != id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm = new LocationAndCenterViewModel
                {
                    CenterList = _db.Center.ToList(),
                    LocationList = _db.Location
                                    .OrderBy(p => p.Name)
                                    .Select(p => p.Name)
                                    .Distinct()
                                    .ToList()
                };

                return View(vm);
            }

            var locationFromDB = _db.Location.Find(vm.Location.ID);
            if (locationFromDB == null)
            {
                return NotFound();
            }

            var locationExists = _db.Location
                    .Where(p => p.Name.Equals(vm.Location.Name) 
                        && p.ID != vm.Location.ID 
                        && p.CenterId == vm.Location.CenterId)
                    .Count();

            if (locationExists > 0)
            {
                StatusMessage = "Error: Location Name already exists.";
                // Error
                vm = new LocationAndCenterViewModel
                {
                    CenterList = _db.Center.ToList(),
                    Location = new Location(),
                    LocationList = _db.Location
                                    .OrderBy(p => p.Name)
                                    .Select(p => p.Name)
                                    .Distinct()
                                    .ToList(),
                    StatusMessage = StatusMessage
                };

                return View(vm);
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            string locationsFolder = @"Photos\Locations";

            locationFromDB.Name = vm.Location.Name;
            locationFromDB.CenterId = vm.Location.CenterId;            

            #region When user upload Picture 1
            if (files[0] != null && files[0].Length > 0)
            {
                if (System.IO.File.Exists(webRootPath + locationFromDB.Picture_1))
                {
                    System.IO.File.Delete(webRootPath + locationFromDB.Picture_1);
                }

                SavingPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, files[0], 0);
            }
            #endregion

            #region When user upload Picture 2
            if (files[1] != null && files[1].Length > 0)
            {
                if (System.IO.File.Exists(webRootPath + locationFromDB.Picture_2))
                {
                    System.IO.File.Delete(webRootPath + locationFromDB.Picture_2);
                }

                SavingPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, files[1], 1);
            }
            #endregion

            #region When user upload Picture 3
            if (files[2] != null && files[2].Length > 0)
            {
                if (System.IO.File.Exists(webRootPath + locationFromDB.Picture_3))
                {
                    System.IO.File.Delete(webRootPath + locationFromDB.Picture_3);
                }

                SavingPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, files[2], 2);
            }
            #endregion

            #region When user upload Picture 4
            if (files[3] != null && files[3].Length > 0)
            {
                if (System.IO.File.Exists(webRootPath + locationFromDB.Picture_4))
                {
                    System.IO.File.Delete(webRootPath + locationFromDB.Picture_4);
                }

                SavingPhotoPathToDBAndWebRoot(locationsFolder, locationFromDB, files[3], 3);
            }
            #endregion

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET Delete Location
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var location = await _db.Location
                    .Include(p => p.Center)
                    .SingleOrDefaultAsync(p => p.ID == id);

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Location/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var locationFromDB = _db.Location.Find(id);
            if (locationFromDB == null)
            {
                return NotFound();
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            
            if (System.IO.File.Exists(webRootPath + locationFromDB.Picture_1))
            {
                System.IO.File.Delete(webRootPath + locationFromDB.Picture_1);
            }

            if (System.IO.File.Exists(webRootPath + locationFromDB.Picture_2))
            {
                System.IO.File.Delete(webRootPath + locationFromDB.Picture_2);
            }

            if (System.IO.File.Exists(webRootPath + locationFromDB.Picture_3))
            {
                System.IO.File.Delete(webRootPath + locationFromDB.Picture_3);
            }

            if (System.IO.File.Exists(webRootPath + locationFromDB.Picture_4))
            {
                System.IO.File.Delete(webRootPath + locationFromDB.Picture_4);
            }

            _db.Location.Remove(locationFromDB);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        private void SavingPhotoPathToDBAndWebRoot(string locationsFolder, Location locationFromDB, IFormFile imageFile, int idx)
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
            string uploadLocation = Path.Combine(webRootPath, locationsFolder);

            if (!System.IO.Directory.Exists(uploadLocation))
                System.IO.Directory.CreateDirectory(uploadLocation);

            var extensionOfFile = fileName.Substring(fileName.LastIndexOf("."),
                           fileName.Length - fileName.LastIndexOf("."));

            string imageFilePath = locationsFolder + @"\" + prevName + locationFromDB.ID + extensionOfFile;

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
                locationFromDB.Picture_1 = imageFilePath;
            }
            else if (idx == 1)
            {
                locationFromDB.Picture_2 = imageFilePath;
            }
            else if (idx == 2)
            {
                locationFromDB.Picture_3 = imageFilePath;
            }
            else
            {
                locationFromDB.Picture_4 = imageFilePath;
            }

        }

        private void SavingDefaultPhotoPathToDBAndWebRoot(string locationsFolder, Location locationFromDB, int idx)
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
            string uploadLocation = Path.Combine(webRootPath, locationsFolder);
            string newImageFilePath = Path.Combine(uploadLocation, prevName + locationFromDB.ID + ".png");

            if (!System.IO.Directory.Exists(uploadLocation))
                System.IO.Directory.CreateDirectory(uploadLocation);

            if (!System.IO.File.Exists(newImageFilePath))
            {
                System.IO.File.Copy(imageFilePath, newImageFilePath);
            }

            newImageFilePath = @"\" + locationsFolder + @"\" + prevName + locationFromDB.ID + ".png";

            if (idx == 0)
            {
                locationFromDB.Picture_1 = newImageFilePath;
            }
            else if (idx == 1)
            {
                locationFromDB.Picture_2 = newImageFilePath;
            }
            else if (idx == 2)
            {
                locationFromDB.Picture_3 = newImageFilePath;
            }
            else
            {
                locationFromDB.Picture_4 = newImageFilePath;
            }
        }


    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GymsHouse.Data;
using GymsHouse.Models;
using GymsHouse.Models.UserViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public InstructorController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Instructor> instructors = await _db.Instructor
                    .Include(p => p.ApplicationUser)
                    .Where(p => p.IsActive == true)
                    .ToListAsync();

            foreach (var instructor in instructors)
            {
                instructor.ApplicationUser.GenderText = ApplicationUser.EGenderType.GetName(typeof(ApplicationUser.EGenderType), instructor.ApplicationUser.Gender);
                instructor.MajorsNames = await GetMajorsNameListByInstructor(instructor.ID);
            }

            return View(instructors);
        }

        // GET: Instructor/Edit/
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var instructor = await _db.Instructor
                                    .SingleOrDefaultAsync(m => m.ID == id);            

            if (instructor == null)
            {
                return NotFound();
            }

            var appUser = await _db.ApplicationUser
                                    .SingleOrDefaultAsync(m => m.Id == instructor.ApplicationUserId);

            instructor.MajorsNames = await GetMajorsNameListByInstructor(id);

            InstructorViewModel vm = new InstructorViewModel
            {
                Instructor = instructor,
                ApplicationUser = appUser,
                MajorsList = await GetMajorsListBySelectedInstructor(id)
            };

            return View(vm);
        }

        // POST Edit Instructor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, InstructorViewModel vm)
        {
            Instructor appInstructor = vm.Instructor;
            List<MajorsListOfSelectedInstructor> appMajors = vm.MajorsList;

            if (id != appInstructor.ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm.Instructor = await _db.Instructor
                                        .SingleOrDefaultAsync(m => m.ID == vm.Instructor.ID);

                vm.ApplicationUser = await _db.ApplicationUser
                                        .SingleOrDefaultAsync(m => m.Id == vm.Instructor.ApplicationUserId);

                return View(vm);
            }

            if (appMajors.Count < 1)
            {
                return BadRequest("Please select at least one major.");
            }

            var instructorFromDB = await _db.Instructor.Where(p => p.ID == id).FirstOrDefaultAsync();
            if (instructorFromDB == null)
            {
                return NotFound();
            }

            // Update Instructor's information
            instructorFromDB.Experiences = appInstructor.Experiences;
            instructorFromDB.History = appInstructor.History;
            instructorFromDB.Awards = appInstructor.Awards;
            instructorFromDB.IsActive = appInstructor.IsActive;

            // Saving Photos
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            string instructorFolder = @"Photos\Instructors";

            if (!System.IO.Directory.Exists(Path.Combine(webRootPath, instructorFolder)))
                System.IO.Directory.CreateDirectory(Path.Combine(webRootPath, instructorFolder));

            #region When user upload Picture 1
            if (files[0] != null && files[0].Length > 0)
            {
                if (System.IO.File.Exists(webRootPath + instructorFromDB.Picture_1))
                {
                    System.IO.File.Delete(webRootPath + instructorFromDB.Picture_1);
                }

                SavingPhotoPathToDBAndWebRoot(instructorFolder, instructorFromDB, files[0], 0);
            }
            #endregion

            #region When user upload Picture 2
            if (files[1] != null && files[1].Length > 0)
            {
                if (System.IO.File.Exists(webRootPath + instructorFromDB.Picture_2))
                {
                    System.IO.File.Delete(webRootPath + instructorFromDB.Picture_2);
                }

                SavingPhotoPathToDBAndWebRoot(instructorFolder, instructorFromDB, files[1], 1);
            }
            #endregion

            #region When user upload Picture 3
            if (files[2] != null && files[2].Length > 0)
            {
                if (System.IO.File.Exists(webRootPath + instructorFromDB.Picture_3))
                {
                    System.IO.File.Delete(webRootPath + instructorFromDB.Picture_3);
                }

                SavingPhotoPathToDBAndWebRoot(instructorFolder, instructorFromDB, files[2], 2);
            }
            #endregion

            #region Update Majors list
            List<MajorOfInstructor> listMajorInstructor = new List<MajorOfInstructor>();

            foreach (var itemMajor in appMajors)
            {
                if (itemMajor.SelectedMajor)
                {
                    MajorOfInstructor new_Major = new MajorOfInstructor
                    {
                        InstructorId = id,
                        MajorId = itemMajor.Id
                    };

                    listMajorInstructor.Add(new_Major);
                }
            }

            if (listMajorInstructor.Count < 1)
            {
                return BadRequest("Please select at least one major.");
            }

            var old_Majors = await _db.MajorOfInstructor
                    .Where(p => p.InstructorId == id)
                    .ToListAsync();

            _db.MajorOfInstructor.RemoveRange(old_Majors);
            await _db.SaveChangesAsync();

            _db.MajorOfInstructor.AddRange(listMajorInstructor);
            await _db.SaveChangesAsync();
            #endregion            

            return RedirectToAction(nameof(Index));
        }

        // GET Details Instructor
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var instructor = await _db.Instructor
                    .Include(p => p.ApplicationUser)
                    .SingleOrDefaultAsync(p => p.ID == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }


        private async Task<string> GetMajorsNameListByInstructor(string instructorId)
        {
            var majorsList = await _db.MajorOfInstructor
                    .Include(p => p.Major)
                    .Where(p => p.InstructorId == instructorId)
                    .ToListAsync();

            var strMajorsList = "";

            foreach (var item in majorsList)
            {
                strMajorsList = strMajorsList + item.Major.Name + ", ";
            }

            if (!string.IsNullOrEmpty(strMajorsList))
            {
                strMajorsList = strMajorsList.Substring(0, strMajorsList.Length - 2);
            }

            return strMajorsList;
        }

        private async Task<List<MajorsListOfSelectedInstructor>> GetMajorsListBySelectedInstructor(string instructorId)
        {
            // Get selected Majors list checked by selected Instructor
            var selectedMajorsIds = await _db.MajorOfInstructor
                                            .Where(p => p.InstructorId == instructorId)
                                            .Select(p => p.MajorId)
                                            .ToListAsync();

            // Get Majors list from Database
            var majorsFromDB = await _db.Major.Distinct().ToListAsync();

            // Return selected Majors within Majors list from Database
            List<MajorsListOfSelectedInstructor> appMajors = new List<MajorsListOfSelectedInstructor>();
            foreach (var itemMajor in majorsFromDB)
            {
                MajorsListOfSelectedInstructor newMajor = new MajorsListOfSelectedInstructor();
                newMajor.Id = itemMajor.ID;
                newMajor.Name = itemMajor.Name;
                newMajor.SelectedMajor = false;

                if (selectedMajorsIds.Exists(str => str.Equals(itemMajor.ID)))
                {
                    newMajor.SelectedMajor = true;
                }

                appMajors.Add(newMajor);
            }

            return appMajors;
        }

        private void SavingPhotoPathToDBAndWebRoot(string instructorFolder, Instructor instructorFromDB, IFormFile imageFile, int idx)
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

            string fileName = imageFile.FileName;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string uploadLocation = Path.Combine(webRootPath, instructorFolder);

            if (!System.IO.Directory.Exists(uploadLocation))
                System.IO.Directory.CreateDirectory(uploadLocation);

            var extensionOfFile = fileName.Substring(fileName.LastIndexOf("."),
                           fileName.Length - fileName.LastIndexOf("."));

            string imageFilePath = instructorFolder + @"\" + prevName + instructorFromDB.ID + extensionOfFile;

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
                instructorFromDB.Picture_1 = imageFilePath;
            }
            else if (idx == 1)
            {
                instructorFromDB.Picture_2 = imageFilePath;
            }
            else if (idx == 2)
            {
                instructorFromDB.Picture_3 = imageFilePath;
            }

        }

    }
}
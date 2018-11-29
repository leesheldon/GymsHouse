using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GymsHouse.Data;
using GymsHouse.Extensions;
using GymsHouse.Models;
using GymsHouse.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymsHouse.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        

        public UsersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var loggedInUser = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (loggedInUser == null)
            {
                return NotFound();
            }

            var usersFromDB = await _db.Users
                .Where(p => p.Id != loggedInUser.Value).ToListAsync();

            foreach (var perUser in usersFromDB)
            {
                perUser.IsLockedOut = await CheckUserIsLockedOut(perUser);
                perUser.RolesNames = await GetRolesNameListBySelectedUser(perUser);
            }

            return View(usersFromDB);
        }

        // GET Edit Users
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _db.ApplicationUser.SingleOrDefaultAsync(p => p.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            // Check selected user is locked out or not? And Get roles name list
            appUser.IsLockedOut = await CheckUserIsLockedOut(appUser);
            appUser.RolesNames = await GetRolesNameListBySelectedUser(appUser);

            UserViewModel vm = new UserViewModel
            {
                SelectedUser = appUser,
                RolesList = await GetRolesListBySelectedUser(id)
            };                        

            return View(vm);
        }

        // POST Edit Users
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel userVM)
        {
            ApplicationUser appUser = userVM.SelectedUser;
            List<RolesListOfSelectedUser> appRoles = userVM.RolesList;

            if (id != appUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (appRoles.Count < 1)
                {
                    return NotFound();
                }

                var userFromDB = await _db.Users.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (userFromDB == null)
                {
                    return NotFound();
                }

                // Update User's information
                userFromDB.FirstName = appUser.FirstName;
                userFromDB.LastName = appUser.LastName;
                userFromDB.PhoneNumber = appUser.PhoneNumber;
                userFromDB.LockoutReason = appUser.LockoutReason;
                userFromDB.AccessFailedCount = appUser.AccessFailedCount;
                userFromDB.LastUpdated = DateTime.Now;
                userFromDB.Gender = appUser.Gender;
                userFromDB.City = appUser.City;
                userFromDB.Country = appUser.Country;
                userFromDB.DateOfBirth = appUser.DateOfBirth;

                if (appUser.LockoutEnd < DateTime.Now)
                {
                    userFromDB.LockoutEnabled = false;
                }

                userFromDB.LockoutEnd = appUser.LockoutEnd;

                #region Update roles list
                List<string> new_Roles = new List<string>();
                bool isInstructor = false;

                foreach (var itemRole in appRoles)
                {
                    if (itemRole.SelectedRole)
                    {
                        new_Roles.Add(itemRole.Name);

                        if (itemRole.Name.Equals(SD.InstructorUser))
                        {
                            isInstructor = true;
                        }
                    }
                }

                if (new_Roles.Count < 1)
                {
                    return BadRequest("Please select at least one role.");
                }

                var old_Roles = await _userManager.GetRolesAsync(appUser);

                var result = await _userManager.RemoveFromRolesAsync(userFromDB, old_Roles);
                if (!result.Succeeded)
                    BadRequest("Failed to remove old roles.");

                result = await _userManager.AddToRolesAsync(userFromDB, new_Roles);
                if (!result.Succeeded)
                    BadRequest("Failed to add new roles.");
                #endregion

                #region Update Instructor information
                var instructorFromDB = await _db.Instructor.Where(p => p.ApplicationUserId == id).FirstOrDefaultAsync();
                if (isInstructor)
                {
                    if (instructorFromDB == null)
                    {
                        // Create new Instructor record
                        Instructor newInstructor = new Instructor();
                        newInstructor.Experiences = "Information is coming soon";
                        newInstructor.ApplicationUserId = id;
                        newInstructor.IsActive = true;

                        _db.Instructor.Add(newInstructor);
                    }
                    else
                    {
                        // Enable this Instructor record
                        instructorFromDB.IsActive = true;
                    }
                }
                else
                {
                    if (instructorFromDB != null)
                    {
                        // Disable this Instructor record
                        instructorFromDB.IsActive = false;
                    }
                }
                #endregion

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(appUser);
        }


        private async Task<string> GetRolesNameListBySelectedUser(ApplicationUser appUser)
        {
            var rolesNameList = await _userManager.GetRolesAsync(appUser);
            var strRolesList = "";

            foreach (var roleItem in rolesNameList)
            {
                strRolesList = strRolesList + roleItem + ", ";
            }

            if (!string.IsNullOrEmpty(strRolesList))
            {
                strRolesList = strRolesList.Substring(0, strRolesList.Length - 2);
            }

            return strRolesList;
        }

        private async Task<bool> CheckUserIsLockedOut(ApplicationUser appUser)
        {
            var isLockedOut = await _userManager.IsLockedOutAsync(appUser);

            return isLockedOut;
        }

        private async Task<List<RolesListOfSelectedUser>> GetRolesListBySelectedUser(string id)
        {
            // Get selected roles list checked by selected user
            var selectedRolesIds = await _db.UserRoles.Where(p => p.UserId == id).Select(p => p.RoleId).ToListAsync();

            // Get Roles list from Database
            var rolesFromDB = await _db.Roles.Distinct().ToListAsync();

            // Return selected Roles within Roles list from Database
            List<RolesListOfSelectedUser> appRoles = new List<RolesListOfSelectedUser>();
            foreach (var itemRole in rolesFromDB)
            {
                RolesListOfSelectedUser newRole = new RolesListOfSelectedUser();
                newRole.Id = itemRole.Id;
                newRole.Name = itemRole.Name;
                newRole.SelectedRole = false;

                if (selectedRolesIds.Exists(str => str.Equals(itemRole.Id)))
                {
                    newRole.SelectedRole = true;
                }

                appRoles.Add(newRole);
            }

            return appRoles;
        }

    }
}
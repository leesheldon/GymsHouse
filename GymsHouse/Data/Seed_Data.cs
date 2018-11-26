using GymsHouse.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Data
{
    public class Seed_Data
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public Seed_Data(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void SeedUser()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<ApplicationUser>>(userData);

                var roles = new List<IdentityRole>
                {
                    new IdentityRole {Name = "NA"},
                    new IdentityRole {Name = "Admin"},
                    new IdentityRole {Name = "TrainingManager"},
                    new IdentityRole {Name = "Instructor"},
                    new IdentityRole {Name = "Staff"},
                    new IdentityRole {Name = "Customer"},                    
                    new IdentityRole {Name = "Member"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    IdentityResult resultUser = _userManager.CreateAsync(user, "Password123+").Result;
                    if (resultUser.Succeeded)
                    {
                        var instructorUser = _userManager.FindByNameAsync(user.UserName).Result;
                        _userManager.AddToRoleAsync(instructorUser, "Instructor").Wait();                                               
                    }
                }

                var adminUser = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "Admin 1"
                };

                IdentityResult resultAdmin = _userManager.CreateAsync(adminUser, "Password123+").Result;

                if (resultAdmin.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync(adminUser.UserName).Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "TrainingManager" }).Wait();
                }
            }
        }

        public void SeedCenters()
        {
            var centersData = System.IO.File.ReadAllText("Data/CentersSeedData.json");
            var centers = JsonConvert.DeserializeObject<List<Center>>(centersData);

            foreach (var center in centers)
            {
                _db.Center.Add(center);
            }

            _db.SaveChanges();
        }

        public void SeedLocations()
        {
            var locationsData = System.IO.File.ReadAllText("Data/LocationsSeedData.json");
            var locations = JsonConvert.DeserializeObject<List<Location>>(locationsData);

            foreach (var location in locations)
            {
                _db.Location.Add(location);
            }

            _db.SaveChanges();
        }

        public void SeedBusinessHours()
        {
            var busHoursData = System.IO.File.ReadAllText("Data/BusHoursSeedData.json");
            var busHours = JsonConvert.DeserializeObject<List<BusinessHours>>(busHoursData);

            foreach (var busHour in busHours)
            {
                _db.BusinessHours.Add(busHour);
            }

            _db.SaveChanges();
        }

        public void SeedMajors()
        {
            var majorsData = System.IO.File.ReadAllText("Data/MajorsSeedData.json");
            var majors = JsonConvert.DeserializeObject<List<Major>>(majorsData);

            foreach (var major in majors)
            {
                _db.Major.Add(major);
            }

            _db.SaveChanges();
        }

        public void SeedInstructors()
        {
            var instructorsData = System.IO.File.ReadAllText("Data/InstructorsSeedData.json");
            var instructors = JsonConvert.DeserializeObject<List<Instructor>>(instructorsData);

            foreach (var instructor in instructors)
            {
                _db.Instructor.Add(instructor);
            }

            _db.SaveChanges();
        }

        public void SeedGymsClasses()
        {
            var gymsClassData = System.IO.File.ReadAllText("Data/GymsClassSeedData.json");
            var gymsClasses = JsonConvert.DeserializeObject<List<GymsClass>>(gymsClassData);

            foreach (var gymsClass in gymsClasses)
            {
                _db.GymsClass.Add(gymsClass);
            }

            _db.SaveChanges();
        }

    }
}

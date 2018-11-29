using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GymsHouse.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public int Gender { get; set; }

        public enum EGenderType { Male = 1, Female = 2, Others = 3 }

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastUpdated { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Picture_1 { get; set; }

        public string Picture_2 { get; set; }

        [Display(Name = "Lockout Reason")]
        public string LockoutReason { get; set; }

        [Display(Name = "UnLock Reason")]
        public string UnLockReason { get; set; }

        public ICollection<Instructor> Instructors { get; set; }

        public ICollection<MembersInClass> MembersInClasses { get; set; }

        [NotMapped]
        public bool IsLockedOut { get; set; }

        [NotMapped]
        public string RolesNames { get; set; }

        [NotMapped]
        public string GenderText { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class Center
    {
        public string ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string Picture_1 { get; set; }

        public string Picture_2 { get; set; }

        public string Picture_3 { get; set; }

        public string Picture_4 { get; set; }

        public ICollection<Location> Locations { get; set; }

        public ICollection<BusinessHours> BusinessHours { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class Major
    {
        public string ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<MajorOfInstructor> MajorsOfInstructors { get; set; }

    }
}

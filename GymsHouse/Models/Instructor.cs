using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class Instructor
    {
        public string ID { get; set; }        

        [Required]
        public string Experiences { get; set; }

        public string History { get; set; }

        public string Awards { get; set; }

        public string Picture_1 { get; set; }

        public string Picture_2 { get; set; }

        public string Picture_3 { get; set; }

        [Required]
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<MajorOfInstructor> MajorsOfInstructors { get; set; }

        public ICollection<ScheduleHeader> ScheduleHeaders { get; set; }

    }
}

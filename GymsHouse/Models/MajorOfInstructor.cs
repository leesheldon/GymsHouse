using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class MajorOfInstructor
    {
        public string ID { get; set; }

        [Required]
        [Display(Name = "Instructor")]
        public string InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }

        [Required]
        [Display(Name = "Major")]
        public string MajorId { get; set; }

        [ForeignKey("MajorId")]
        public Major Major { get; set; }
                
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class ScheduleHeader
    {        
        public string ID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
                
        [Required]
        public int ClassSize { get; set; }

        public string Notes { get; set; }

        [Required]
        [Display(Name = "Class")]
        public string ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual GymsClass GymsClass { get; set; }

        [Required]
        [Display(Name = "Instructor")]
        public string InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public virtual Instructor Instructor { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        public ICollection<ScheduleDetails> ScheduleDetails { get; set; }

        public ICollection<MembersInClass> MembersInClasses { get; set; }

    }
}

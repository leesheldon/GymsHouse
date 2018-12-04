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

        public string Status { get; set; }

        public enum EStatus { NA = 0, Waiting = 1, Started = 2, Paused = 3, Cancelled = 4, Finished = 5 }

        public bool Monday { get; set; }

        public bool Tuesday { get; set; }

        public bool Wednesday { get; set; }

        public bool Thursday { get; set; }

        public bool Friday { get; set; }

        public bool Saturday { get; set; }

        public bool Sunday { get; set; }


        [Required]
        [Display(Name = "Class")]
        public string ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual TrainingClass TrainingClass { get; set; }

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

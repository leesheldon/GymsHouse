using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class MembersInClass
    {
        public string ID { get; set; }

        public string Status { get; set; }

        public enum EStatus { NA = 0, Submitted = 1, Enrolled = 2, Active = 3, Cancelled = 4, Finished = 5, Trial = 6 }

        public string Comments { get; set; }

        public int Rating { get; set; }

        [Required]
        [Display(Name = "Schedule Header")]
        public string ScheduleHeaderId { get; set; }

        [ForeignKey("ScheduleHeaderId")]
        public virtual ScheduleHeader ScheduleHeader { get; set; }

        [Required]
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}

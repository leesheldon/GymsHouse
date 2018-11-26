using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class ScheduleDetails
    {
        public string ID { get; set; }

        [Required]
        [StringLength(7)]
        public string From { get; set; }

        [Required]
        [StringLength(7)]
        public string To { get; set; }

        [Required]
        [StringLength(10)]
        public string DayOfWeek { get; set; }

        [Required]
        [Display(Name = "Schedule Header")]
        public string ScheduleHeaderId { get; set; }

        [ForeignKey("ScheduleHeaderId")]
        public virtual ScheduleHeader ScheduleHeader { get; set; }

    }
}

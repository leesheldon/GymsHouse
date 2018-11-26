using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class BusinessHours
    {
        public string ID { get; set; }

        [Required]
        public string DaysOfWeek { get; set; }

        [Required]
        [StringLength(7)]
        public string From { get; set; }

        [Required]
        [StringLength(7)]
        public string To { get; set; }

        public bool IsClosed { get; set; }

        [Required]
        [Display(Name = "Center")]
        public string CenterId { get; set; }

        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; }

    }
}

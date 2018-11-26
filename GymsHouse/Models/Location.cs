using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class Location
    {
        public string ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Picture_1 { get; set; }

        public string Picture_2 { get; set; }

        public string Picture_3 { get; set; }

        public string Picture_4 { get; set; }

        [Required]
        [Display(Name = "Center")]
        public string CenterId { get; set; }

        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; }

        public ICollection<ScheduleHeader> ScheduleHeaders { get; set; }

    }
}

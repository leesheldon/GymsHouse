using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class GymsClass
    {
        public string ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public double Duration { get; set; }

        public string Picture_1 { get; set; }

        public string Picture_2 { get; set; }

        public string Picture_3 { get; set; }

        public string Picture_4 { get; set; }

        public ICollection<ScheduleHeader> ScheduleHeaders { get; set; }

    }
}

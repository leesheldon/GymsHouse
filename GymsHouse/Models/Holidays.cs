using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class Holidays
    {
        public string ID { get; set; }

        public DateTime Holiday { get; set; }

        [Required]
        [Display(Name = "Center")]
        public string CenterId { get; set; }

        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; }

    }
}

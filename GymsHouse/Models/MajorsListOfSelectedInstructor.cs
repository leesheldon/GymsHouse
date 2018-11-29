using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    [NotMapped]
    public class MajorsListOfSelectedInstructor
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool SelectedMajor { get; set; }

    }
}

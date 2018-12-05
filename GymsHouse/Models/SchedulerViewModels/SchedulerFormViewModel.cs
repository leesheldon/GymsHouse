using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.SchedulerViewModels
{
    public class SchedulerFormViewModel
    {
        public ScheduleHeader ScheduleHeader { get; set; }

        public IEnumerable<TrainingClass> TrainingClassesList { get; set; }

        public IEnumerable<Instructor> InstructorsList { get; set; }
        
        public string SelectedLocation { get; set; }
        public IEnumerable<SelectListItem> LocationsList { get; set; }

        public string SelectedCenter { get; set; }
        public IEnumerable<SelectListItem> CentersList { get; set; }

        public string ErrorMessage { get; set; }

    }
}

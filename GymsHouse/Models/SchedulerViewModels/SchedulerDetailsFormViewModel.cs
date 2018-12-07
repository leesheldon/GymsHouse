using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.SchedulerViewModels
{
    public class SchedulerDetailsFormViewModel
    {
        public ScheduleHeader ScheduleHeader { get; set; }

        public IEnumerable<ScheduleDetails> ScheduleDetailsList { get; set; }

        public ScheduleDetails NewScheduleDetails { get; set; }

        
        public string SelectedFromText { get; set; }
        public List<SelectListItem> FromList { get; set; }

        public string SelectedToText { get; set; }
        public List<SelectListItem> ToList { get; set; }

        public string SelectedDaysText { get; set; }
        public List<SelectListItem> DaysList { get; set; }


        public string ErrorMessage { get; set; }

    }
}

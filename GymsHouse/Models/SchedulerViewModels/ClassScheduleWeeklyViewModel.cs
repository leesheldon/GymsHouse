using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.SchedulerViewModels
{
    public class ClassScheduleWeeklyViewModel
    {
        public IEnumerable<ScheduleDetails> ScheduleOnSunday { get; set; }

        public IEnumerable<ScheduleDetails> ScheduleOnMonday { get; set; }

        public IEnumerable<ScheduleDetails> ScheduleOnTuesday { get; set; }

        public IEnumerable<ScheduleDetails> ScheduleOnWednesday { get; set; }

        public IEnumerable<ScheduleDetails> ScheduleOnThursday { get; set; }

        public IEnumerable<ScheduleDetails> ScheduleOnFriday { get; set; }

        public IEnumerable<ScheduleDetails> ScheduleOnSaturday { get; set; }

    }
}

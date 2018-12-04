using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.SchedulerViewModels
{
    public class HolidaysAndCenterViewModel
    {
        public Holidays Holidays { get; set; }

        public IEnumerable<Center> CenterList { get; set; }
        
        public string StatusMessage { get; set; }

    }
}

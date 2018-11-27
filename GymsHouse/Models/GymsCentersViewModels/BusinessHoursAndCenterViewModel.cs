using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.GymsCentersViewModels
{
    public class BusinessHoursAndCenterViewModel
    {
        public BusinessHours BusinessHours { get; set; }

        public IEnumerable<Center> CenterList { get; set; }
                
        public string StatusMessage { get; set; }

    }
}

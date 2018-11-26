using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.LocationViewModels
{
    public class LocationAndCenterViewModel
    {
        public Location Location { get; set; }

        public IEnumerable<Center> CenterList { get; set; }

        public List<string> LocationList { get; set; }

        public string StatusMessage { get; set; }

    }
}

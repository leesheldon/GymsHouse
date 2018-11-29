using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.UserViewModels
{
    public class InstructorViewModel
    {
        public Instructor Instructor { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public List<MajorsListOfSelectedInstructor> MajorsList { get; set; }        
                
    }
}

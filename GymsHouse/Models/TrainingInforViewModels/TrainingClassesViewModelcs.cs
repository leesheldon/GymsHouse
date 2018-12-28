using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.TrainingInforViewModels
{
    public class TrainingClassesViewModelcs
    {
        public IEnumerable<TrainingClass> TrainingClassesList { get; set; }

        public IEnumerable<Instructor> InstructorsList { get; set; }


    }
}

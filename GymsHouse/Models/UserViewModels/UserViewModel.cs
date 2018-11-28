using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models.UserViewModels
{
    public class UserViewModel
    {
        public ApplicationUser SelectedUser { get; set; }

        public List<RolesListOfSelectedUser> RolesList { get; set; }

    }
}

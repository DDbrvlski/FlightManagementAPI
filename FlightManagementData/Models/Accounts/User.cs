using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagementData.Models.Accounts
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}

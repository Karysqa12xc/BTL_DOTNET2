using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BTL_DOTNET2.Models
{
    public class UserAndRoles
    {
        public List<User> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
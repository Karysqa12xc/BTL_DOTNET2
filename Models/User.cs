using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BTL_DOTNET2.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string UserId{ get; set; }
        public User()
        {
            UserId = Guid.NewGuid().ToString();
        }
    }
}
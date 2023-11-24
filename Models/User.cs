using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BTL_DOTNET2.Models
{
    public class User
    {
        [Key]        
        public int UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }        
        public string Avatar { get; set; }  
    }
}
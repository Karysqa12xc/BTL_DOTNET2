using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class ContentPost
    {
        [Key]
        public int ContentPostId { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập nội dung")]
        public string Paragram { get; set; }
        public string Image { get; set; }
        public List<Post> Posts { get; set; }
    }
}
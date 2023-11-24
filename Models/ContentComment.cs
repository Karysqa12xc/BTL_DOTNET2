using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class ContentComment
    {
        [Key]
        public int ContentCommentId { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập nội dung")]
        public string Paragram { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
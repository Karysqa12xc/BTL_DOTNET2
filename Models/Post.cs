using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class Post
    {
        [Key]
        public int postId { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập tiêu đề")]
        public string Title { get; set; }
        public DateTime PostTime { get; set; } = DateTime.Now;
        public int CommentTotal { get; set; } = 0;
        public bool isSave { get; set; }
        public int UserId { get; set; }
        public int CateId { get; set; }
        public int ContentPostId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
        [ForeignKey("CateId")]
        public Categories categories;
        [ForeignKey("ContentPostId")]
        public ContentPost ContentPost { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Notifications> Notifications { get; set; }
    }
}
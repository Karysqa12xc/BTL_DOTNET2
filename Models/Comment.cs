using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.Now;
        public int UserId{ get; set; }
        public int postId{ get; set; }
        public int ContentCommentId{ get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("postId")]
        public Post Post { get; set; }
        [ForeignKey("ContentCommentId")]
        public ContentComment ContentComment { get; set; }   
    }
}
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

        public DateTime? CommentTime { get; set; } = DateTime.Now;

        [Column("postId")]
        public int PostId { get; set; }

        public int ContentCommentId { get; set; }

        [ForeignKey("ContentCommentId")]
        [InverseProperty("Comments")]
        public virtual ContentComment ContentComment { get; set; } = null!;

        [ForeignKey("PostId")]
        [InverseProperty("Comments")]
        public virtual Post Post { get; set; } = null!;
        public User? User { get; set; }
    }
}
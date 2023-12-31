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
        [Column("postId")]
        public int PostId { get; set; }

        public string Title { get; set; } = null!;

        public DateTime? PostTime { get; set; } = DateTime.Now;

        public int? CommentTotal { get; set; } = 0;

        [Column("isChecked")]
        public bool IsChecked { get; set; } = false;
        public int CateId { get; set; }

        public int ContentPostId { get; set; }

        [ForeignKey("CateId")]
        [InverseProperty("Posts")]
        public virtual Category Cate { get; set; } = null!;

        [InverseProperty("Post")]
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [ForeignKey("ContentPostId")]
        [InverseProperty("Posts")]
        public virtual ContentPost ContentPost { get; set; } = null!;

        [InverseProperty("Post")]
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public User? User { get; set; }  
    }


}
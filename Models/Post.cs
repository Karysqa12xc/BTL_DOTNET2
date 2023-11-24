using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Models;

public partial class Post
{
    [Key]
    [Column("postId")]
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime PostTime { get; set; }

    public int CommentTotal { get; set; }

    [Column("isSave")]
    public bool IsSave { get; set; }

    public int UserId { get; set; }

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

    [ForeignKey("UserId")]
    [InverseProperty("Posts")]
    public virtual User User { get; set; } = null!;
}

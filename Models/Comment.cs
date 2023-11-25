using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Models;

public partial class Comment
{
    [Key]
    public int CommentId { get; set; }

    public DateTime CommentTime { get; set; }

    public int UserId { get; set; }

    [Column("postId")]
    public int PostId { get; set; }

    public int ContentCommentId { get; set; }

    [ForeignKey("ContentCommentId")]
    [InverseProperty("Comments")]
    public virtual ContentComment ContentComment { get; set; } = null!;

    [ForeignKey("PostId")]
    [InverseProperty("Comments")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Comments")]
    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Models;

public partial class ContentComment
{
    [Key]
    public int ContentCommentId { get; set; }

    public string Paragram { get; set; } = null!;

    public string Image { get; set; } = null!;

    [InverseProperty("CommentNavigation")]
    public virtual Comment? Comment { get; set; }
}

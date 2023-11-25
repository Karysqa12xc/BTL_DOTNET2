using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Models;

public partial class ContentPost
{
    [Key]
    public int ContentPostId { get; set; }

    public string Paragram { get; set; } = null!;

    public string Image { get; set; } = null!;

    [InverseProperty("ContentPost")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}

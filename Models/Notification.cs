using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Models;

public partial class Notification
{
    [Key]
    public int NotiId { get; set; }

    public int UserId { get; set; }

    [Column("postId")]
    public int PostId { get; set; }

    public DateTime Time { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Notifications")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}

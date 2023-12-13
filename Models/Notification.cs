using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class Notification
    {
        [Key]
        public int NotiId { get; set; }

        [Column("postId")]
        public int PostId { get; set; }

        public DateTime? Time { get; set; } = DateTime.Now;

        [ForeignKey("PostId")]
        [InverseProperty("Notifications")]
        public virtual Post Post { get; set; } = null!;
        public User? User { get; set; }
    }
}
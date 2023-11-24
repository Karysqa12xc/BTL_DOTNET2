using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class Notifications
    {
        [Key]
        public int NotiId { get; set; }
        public int UserId { get; set; }
        public int postId { get; set; }
        [ForeignKey("postId")]
        public Post Post { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
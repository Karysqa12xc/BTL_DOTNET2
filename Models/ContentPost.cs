using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class ContentPost
    {
        [Key]
        public int ContentPostId { get; set; }

        public string Paragram { get; set; } = null!;

        public string? Image { get; set; }

        [InverseProperty("ContentPost")]
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();        
        
        
    }
}
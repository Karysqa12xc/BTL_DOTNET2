using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class ContentComment
    {
        [Key]
        public int ContentCommentId { get; set; }

        public string Paragraph { get; set; } = null!;

        public string Image { get; set; } = null!;

        [InverseProperty("ContentComment")]
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
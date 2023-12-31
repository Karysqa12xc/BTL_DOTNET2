using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;

namespace BTL_DOTNET2.Models
{
    public class ContentTotal
    {
        [Key]
        public int MediaId { get; set; }
        public MediaType MediaType { get; set; }
        public string? Path { get; set; }
        public int? ContentPostId { get; set; }
        [ForeignKey("ContentPostId")]
        [InverseProperty("Media")]
        public virtual ContentPost ContentPost { get; set; } = null!;
        public int? ContentCommentId { get; set; }
        [ForeignKey("ContentCommentId")]
        [InverseProperty("Media")]
        public virtual ContentComment ContentComment { get; set; } = null!;        
        public bool IsSelected { get; set; }
        
        
    }
    public enum MediaType
    {
        Image,
        Video,
    }
}
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTL_DOTNET2.Models
{
    public class PostCommentContentViewModel
    {

        public Post Post { get; set; } = null!;
        public ContentPost ContentPost { get; set; } = null!;
        public Comment Comment { get; set; } = null!;
        public List<Comment> Comments { get; set; } = null!;
        public ContentComment ContentComment { get; set; } = null!;
        public SelectList Category { get; set; } = null!;
        public IFormFile? ImgUrl { get; set; }
        public IFormFile? VideoUrl { get; set; }
        public List<IFormFile>? ImgUrls { get; set; }
        public List<IFormFile>? VideoUrls { get; set; }
        public List<ContentTotal> MediaContentPost { get; set; } = new List<ContentTotal>();
        public List<ContentTotal> MediaContentComment { get; set; } = new List<ContentTotal>();
        public class CommentWithMedia
        {
            public Comment Comment { get; set; } = null!;
            public List<ContentTotal> Media { get; set; } = null!;
        }
        public List<CommentWithMedia> CommentWithMedias { get; set; } = null!;
  
    }
}
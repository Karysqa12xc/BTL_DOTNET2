using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTL_DOTNET2.Models
{
    public class PostContentViewModel
    {

        public Post Post { get; set; } = null!;
        public ContentPost ContentPost { get; set; } = null!;
        public Comment Comment { get; set; } = null!;
        public List<Comment> Comments { get; set; } = null!;
        public ContentComment ContentComment { get; set; } = null!;
        public SelectList Category { get; set; } = null!;
        public IFormFile? ImgUrl { get; set; }
        
        
        
    }
}
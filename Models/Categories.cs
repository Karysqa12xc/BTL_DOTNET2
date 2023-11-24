using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class Categories
    {
        [Key]
        public int CateId { get; set; }
        public string CateName { get; set; }
        public List<Post> Posts { get; set; }
    }
}
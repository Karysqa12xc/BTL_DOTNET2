using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_DOTNET2.Models
{
    public class Category
    {
        [Key]
        public int CateId { get; set; }

        public string CateName { get; set; } = null!;

        [InverseProperty("Cate")]
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
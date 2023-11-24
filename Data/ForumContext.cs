using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_DOTNET2.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Data
{
    public class ForumContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<ContentComment> ContentComments { get; set; }
        public DbSet<ContentPost> ContentPosts { get; set; }
        public ForumContext(DbContextOptions<ForumContext> options) : base(options) { }
    }
}
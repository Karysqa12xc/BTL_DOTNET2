using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }
    public DbSet<User> users { get; set; }
    public virtual DbSet<ContentComment> ContentComments { get; set; }

    public virtual DbSet<ContentPost> ContentPosts { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public ApplicationDbContext()
    {
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasIndex(b=> b.UserId).IsUnique();
    }
}

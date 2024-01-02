using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<ContentComment> ContentComments { get; set; }
    public virtual DbSet<ContentPost> ContentPosts { get; set; }
    public virtual DbSet<ContentTotal> ContentTotals { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<User> Users { get; set; }
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
            .HasIndex(b => b.UserId).IsUnique();

        modelBuilder.Entity<Post>()
        .Property(e => e.PostTime)
        .IsRequired(false);

        modelBuilder.Entity<Post>()
        .Property(e => e.CommentTotal)
        .IsRequired(false);

        modelBuilder.Entity<Comment>()
        .Property(c => c.CommentTime)
        .IsRequired(false);

        modelBuilder.Entity<Notification>()
        .Property(n => n.Time)
        .IsRequired(false);

        modelBuilder.Entity<ContentTotal>()
        .HasOne(ct => ct.ContentPost)
        .WithMany(cp => cp.Media)
        .HasForeignKey(ct => ct.ContentPostId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ContentTotal>()
        .HasOne(ct => ct.ContentComment)
        .WithMany(cp => cp.Media)
        .HasForeignKey(ct => ct.ContentCommentId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Post>()
        .HasMany(p => p.Comments)
        .WithOne(c => c.Post)
        .HasForeignKey(c => c.PostId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ContentComment>()
            .HasMany(c => c.Comments)
            .WithOne(cc => cc.ContentComment)
            .HasForeignKey(c => c.ContentCommentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ContentComment>()
            .HasMany(cc => cc.Media)
            .WithOne(mt => mt.ContentComment)
            .HasForeignKey(mt => mt.ContentCommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

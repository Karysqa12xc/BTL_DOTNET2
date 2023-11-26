using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_DOTNET2.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ContentPost> ContentPosts { get; set; }
        public DbSet<ContentComment> ContentComments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.ContentComment).WithMany(p => p.Comments)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_ContentComments");

                entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Posts");

                entity.HasOne(d => d.User).WithMany(p => p.Comments)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Users");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(d => d.Post).WithMany(p => p.Notifications)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_Posts");

                entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_Users");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.Cate).WithMany(p => p.Posts)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_Categories");

                entity.HasOne(d => d.ContentPost).WithMany(p => p.Posts)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_ContentPosts");

                entity.HasOne(d => d.User).WithMany(p => p.Posts)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_Users");
            });
        }
    }
}

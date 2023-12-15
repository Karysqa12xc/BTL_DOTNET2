﻿using BTL_DOTNET2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }
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
            .HasIndex(b => b.UserId).IsUnique();

        modelBuilder.
        Entity<ContentComment>()
        .Property(e => e.Image)
        .IsRequired(false);

        modelBuilder.Entity<ContentPost>()
        .Property(e=> e.Image)
        .IsRequired(false);

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
    }
}

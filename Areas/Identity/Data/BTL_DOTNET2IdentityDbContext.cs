using BTL_DOTNET2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTL_DOTNET2.Areas.Identity.Data;

public class BTL_DOTNET2IdentityDbContext : IdentityDbContext<ForumUser>
{
    public BTL_DOTNET2IdentityDbContext(DbContextOptions<BTL_DOTNET2IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}

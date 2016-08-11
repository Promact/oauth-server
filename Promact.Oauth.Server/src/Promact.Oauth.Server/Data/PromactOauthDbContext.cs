using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.Models;

namespace Promact.Oauth.Server.Data
{
    public class PromactOauthDbContext : IdentityDbContext<ApplicationUser>
    {
        public PromactOauthDbContext(DbContextOptions<PromactOauthDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<OAuth> OAuth { get; set; }
        public DbSet<ConsumerApps> ConsumerApps { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");


        }
        
    }
}

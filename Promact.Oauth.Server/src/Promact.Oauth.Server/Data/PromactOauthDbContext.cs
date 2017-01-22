using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
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
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            
        }
        
    }
}

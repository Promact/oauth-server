using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Configuration;
using Promact.Oauth.Server.Configuration.DefaultAPIResource;
using Promact.Oauth.Server.Configuration.DefaultIdentityResource;
using System.Linq;

namespace Promact.Oauth.Server.Seed
{
    public class IdentityServerInitialize
    {
        #region Public Method
        /// <summary>
        /// Method used to migrate PersistedGrantDbContext and ConfigurationDbContext of IdentityServer 
        /// and store predefined value of API Resource and Identity Resource
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="defaultApiResource">IDefaultApiResources</param>
        /// <param name="defaultIdentityResource">IDefaultIdentityResources</param>
        public void InitializeDatabase(IApplicationBuilder app, IDefaultApiResources defaultApiResource, IDefaultIdentityResources defaultIdentityResource)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                // Migrating PersistedGrantDbContext and ConfigurationDbContext of IdentityServer
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                // Adding Predefined value of API Resource
                if (!context.ApiResources.Any())
                {
                    foreach (var resource in defaultApiResource.GetDefaultApiResource())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                // Adding Predefined value of Identity Resource
                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in defaultIdentityResource.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.Clients.Any())
                {
                    foreach (var resource in RandomClient.AddRandomClients())
                    {
                        context.Clients.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
        #endregion
    }
}

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        public void InitializeDatabaseForPreDefinedAPIResourceAndIdentityResources(IApplicationBuilder app, IDefaultApiResources defaultApiResource, IDefaultIdentityResources defaultIdentityResource)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configurationDbContext.Database.Migrate();

                //Adding Predefined value of API Resource
                if (!configurationDbContext.ApiResources.Any())
                {
                    foreach (var resource in defaultApiResource.GetDefaultApiResource())
                    {
                        configurationDbContext.ApiResources.Add(resource.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                // Adding Predefined value of Identity Resource
                if (!configurationDbContext.IdentityResources.Any())
                {
                    foreach (var resource in defaultIdentityResource.GetIdentityResources())
                    {
                        configurationDbContext.IdentityResources.Add(resource.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
            }
        }
        #endregion
    }
}

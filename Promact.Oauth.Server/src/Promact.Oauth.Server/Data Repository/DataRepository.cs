using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Data_Repository
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        private PromactOauthDbContext promactDbContext;
        private DbSet<T> dbSet;

        public DataRepository(PromactOauthDbContext _promactOAuthDbContext)
        {
            promactDbContext = _promactOAuthDbContext;
            dbSet = promactDbContext.Set<T>();
        }

        /// <summary>
        /// Adds new employee
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            dbSet.Add(entity);
            promactDbContext.SaveChanges();
        }

        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> List()
        {
            return dbSet.AsEnumerable();
        }


        /// <summary>
        /// Edits the user details
        /// </summary>
        public void Update(T entity)
        {
            promactDbContext.Entry(entity).State = EntityState.Modified;
            promactDbContext.SaveChanges();
        }

        
        /// <summary>
        /// Saves the changes of the database context
        /// </summary>
        public void Save()
        {
            promactDbContext.SaveChanges();
        }
    }
}

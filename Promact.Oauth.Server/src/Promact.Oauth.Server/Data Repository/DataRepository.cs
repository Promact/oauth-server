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
        private PromactOauthDbContext _promactDbContext;
        private DbSet<T> dbSet;

        public DataRepository(PromactOauthDbContext promactOAuthDbContext)
        {
            _promactDbContext = promactOAuthDbContext;
            dbSet = _promactDbContext.Set<T>();
        }

        /// <summary>
        /// Adds new entry to the database
        /// </summary>
        /// <param name="entity">entity</param>
        public void Add(T entity)
        {
            dbSet.Add(entity);
            _promactDbContext.SaveChanges();
        }

        /// <summary>
        /// Fetches the list of all entries
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> List()
        {
            return dbSet.AsEnumerable();
        }


        /// <summary>
        /// Updates the database with updated details of an entry
        /// </summary>
        /// <param name="entity">entity</param>
        public void Update(T entity)
        {
            _promactDbContext.Entry(entity).State = EntityState.Modified;
            _promactDbContext.SaveChanges();
        }

        
        /// <summary>
        /// Saves the changes of the database context
        /// </summary>
        public void Save()
        {
            _promactDbContext.SaveChanges();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Data_Repository
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        #region "Private Member(s)"

        private readonly PromactOauthDbContext _promactDbContext;
        private readonly DbSet<T> _dbSet;

        #endregion

        #region Public Constructor

        public DataRepository(PromactOauthDbContext promactOAuthDbContext)
        {
            _promactDbContext = promactOAuthDbContext;
            _dbSet = _promactDbContext.Set<T>();
        }

        #endregion

        #region "Public properties"

        /// <summary>
        /// Property fetches the total Count from the dbset.
        /// </summary>
        public int TotalCount
        {
            get { return _dbSet.Count(); }
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// Adds new entry to the database
        /// </summary>
        /// <param name="entity">entity</param>
        public void Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                _promactDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Add new entry to the database using Async
        /// </summary>
        /// <param name="entity"></param>
        public void AddAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                //_promactDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Fetches the list of all entries
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> List()
        {
            return _dbSet.AsEnumerable();
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
        /// Updates the database with updated details of an entry
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateAsync(T entity)
        {
            try
            {
                _promactDbContext.Entry(entity).State = EntityState.Modified;
                //_promactDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Saves the changes of the database context
        /// </summary>
        public void Save()
        {
            _promactDbContext.SaveChanges();
        }
        /// <summary>
        /// Saves the changes of the database using Async
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            { return await _promactDbContext.SaveChangesAsync(); }
            catch (Exception)
            { throw; }
        }
        /// <summary>
        /// Method attaches the entity from the context
        /// </summary>
        /// <param name="entity"></param>
        public void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        /// <summary>
        /// Method fetches the IQueryable based on expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }


        /// <summary>
        /// Method fetches the IQueryable based on expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        

        public async Task<IEnumerable<T>> FetchAsync(Expression<Func<T, bool>> predicate)
        {
          
                return await _dbSet.Where(predicate).ToListAsync();
            
        }

        /// <summary>
        /// Method fetches the first or default item from the datacontext based on the the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {

            return _dbSet.FirstOrDefault<T>(predicate);
        }

        /// <summary>
        /// Method fetches the first record based on the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.First<T>(predicate);
        }

        /// <summary>
        /// Method fetches the first item from the datacontext based on the the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstAsync<T>(predicate);
        }


        /// <summary>
        /// Method Fetches the particular single record based on the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Single<T>(predicate);
        }

        /// <summary>
        /// Method Fetches all the data before executing query.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }


        /// <summary>
        /// This method is used to get maximum id.
        /// </summary>
        /// <returns></returns>
        public long GetMaxId()
        {
            var obj = new object();
            lock (obj)
            {
                return _dbSet.Count() + 1;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (_promactDbContext.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entitiesToDelete = Fetch(predicate);
                foreach (var entity in entitiesToDelete)
                {
                    if (_promactDbContext.Entry(entity).State == EntityState.Detached)
                    {
                        _dbSet.Attach(entity);
                    }
                    _dbSet.Remove(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method fetches the first or default item from the datacontext based on the the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(predicate);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool disposed = false;

        /// <summary>
        /// Dispose Method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _promactDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        ~DataRepository()
        {
            Dispose();
        }
    }
}

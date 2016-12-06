using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Data_Repository
{
    public interface IDataRepository<T> :IDisposable
    {
        /// <summary>
        /// Add the new entity into the datacontext
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);
        /// <summary>
        /// Add the new entity into the datacontext
        /// </summary>
        /// <param name="entity"></param>
        void AddAsync(T entity);
        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> List();

        /// <summary>
        /// Update the entity into the datacontext.
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Update the entity into the datacontext.
        /// </summary>
        /// <param name="entity"></param>
        void UpdateAsync(T entity);

        /// <summary> 
        /// Save changes into the database.
        /// </summary>
        void Save();

        /// <summary>
        /// Save changes into the database.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Property gets the Entity count.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Attaches the entity into the datacontext.
        /// </summary>
        /// <param name="entity"></param>
        void Attach(T entity);


        /// <summary>
        ///   Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate"> Specified a filter </param>
        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate);


        /// <summary>
        ///   Find object by specified expression.
        /// </summary>
        /// <param name="predicate"> </param>
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Method fetches the first record based on the supplied function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T First(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Fetches the Single entity based on the function.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T Single(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Fetches all the item from the datacontext.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();


        /// <summary>
        /// This method is used to get maximum id.
        /// </summary>
        /// <returns></returns>
        long GetMaxId();

        /// <summary>
        ///   Delete the object from database.
        /// </summary>
        /// <param name="entity"> Specified a existing object to delete. </param>
        void Delete(T entity);

        /// <summary>
        ///   Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"> </param>
        void Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///  Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///  Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FetchAsync(Expression<Func<T, bool>> predicate);
    }
}

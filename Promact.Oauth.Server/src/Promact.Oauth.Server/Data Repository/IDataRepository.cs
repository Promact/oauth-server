using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Data_Repository
{
    public interface IDataRepository<T> 
    {
        /// <summary>
        /// Add the new entity into the datacontext
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);


        IEnumerable<T> List();

        /// <summary>
        /// Update the entity into the datacontext.
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Save changes into the database.
        /// </summary>
        void Save();

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
        /// 
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
        
    }
}

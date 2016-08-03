using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Data_Repository
{
    public interface IDataRepository<T> 
    {
        void Add(T entity);
        IEnumerable<T> List();
        void Update(T entity);
        void Save();
    }
}

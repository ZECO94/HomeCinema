using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        IEnumerable<T> Get(Expression<Func<T, bool>>? expression
            , params Expression<Func<T, object>>[] includes);
        T? GetOne(Expression<Func<T, bool>>? expression
            , params Expression<Func<T, object>>[] includes);
    }
}

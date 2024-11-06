using HomeCinema.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> dbset;
        public Repository(ApplicationDbContext _context)
        {
            this._context = _context;
            dbset = _context.Set<T>();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbset.AddRange(entities);
            _context.SaveChanges();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>>? expression, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbset;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return query.ToList();

        }

        public T? GetOne(Expression<Func<T, bool>>? expression, params Expression<Func<T, object>>[] includes)
        {
            return Get(expression, includes).FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            dbset.Update(entity);
            _context.SaveChanges();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            dbset.UpdateRange(entities);
            _context.SaveChanges();
        }
    }
}

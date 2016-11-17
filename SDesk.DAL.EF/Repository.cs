using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace SDesk.DAL.EF
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbset;

        public Repository(DbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public void Delete(T entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        public void Update(T entity)
        {
            //var entry = _context.Entry(entity);
            //if (entry.State == EntityState.Detached)
            //    _dbset.Attach(entity);

            //entry.State = EntityState.Modified;
            _dbset.AddOrUpdate(entity); 
        }

        public T GetById(long id)
        {
            return _dbset.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }
    }
}

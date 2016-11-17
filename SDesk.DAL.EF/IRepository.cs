using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SDesk.DAL.EF
{
    public interface IRepository<T> where T : class
    {
        bool Add(T entity);
        bool Delete(T entity);
        bool Update(T entity);
        T GetById(long id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}

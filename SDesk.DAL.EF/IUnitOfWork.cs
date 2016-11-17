using System;

namespace SDesk.DAL.EF
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        void Save();
    }
}

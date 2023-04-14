using System.Linq.Expressions;

namespace DynamicaLabs.HubspotIntegration.Interfaces
{
    internal interface IRepository<T> where T : class
    {
        T Add(T item);
        void Update(T item);
        Task<IQueryable<T>> GetAllAsync();
        Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task SaveAsync();
    }
}

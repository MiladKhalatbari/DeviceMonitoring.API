using System.Linq.Expressions;

namespace DeviceMonitoring.Data.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task<T> GetAsync(int id);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
}
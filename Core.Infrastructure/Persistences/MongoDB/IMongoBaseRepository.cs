using Core.Infrastructure.Domain.BaseEntities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Sample.Api.Infrastructure
{
    public interface IMongoBaseRepository<T> where T : IEntity<Guid>
    {
        IMongoCollection<T> Set { get; }

        Task AddAsync(T entity);

        Task<IReadOnlyCollection<T>> GetAllAsync();

        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        Task<T> GetAsync(Guid id);

        Task<T> GetAsync(Expression<Func<T, bool>> filter);

        Task RemoveAsync(Guid id);

        Task UpdateAsync(T entity);
    }
}
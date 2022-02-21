using Core.Infrastructure.Domain.BaseEntities;
using MongoDB.Driver;
using Sample.Api.Infrastructure;
using System.Linq.Expressions;

namespace Core.Infrastructure.Persistences.MongoDB
{
    public class MongoBaseRepository<T> : IMongoBaseRepository<T> where T : IEntity<Guid>
    {

        #region Properties

        protected readonly IMongoCollection<T> _collection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        /// <summary>
        /// Public mongo collection for doing custom queries on collection if needs.
        /// </summary>
        public IMongoCollection<T> Set { get; }
        #endregion

        #region Ctor

        public MongoBaseRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
            Set = database.GetCollection<T>(collectionName);
        }

        #endregion

        #region GetAllAsync

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _collection.Find(filterBuilder.Empty).ToListAsync();
        }

        #endregion

        #region GetAllAsync

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        #endregion

        #region GetAsync

        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        #endregion

        #region AddAsync

        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _collection.InsertOneAsync(entity);
        }

        #endregion

        #region UpdateAsync

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var filter = filterBuilder.Eq(e => e.Id, entity.Id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        #endregion

        #region RemoveAsync

        public async Task RemoveAsync(Guid id)
        {
            var filter = filterBuilder.Eq(e => e.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        #endregion

    }
}
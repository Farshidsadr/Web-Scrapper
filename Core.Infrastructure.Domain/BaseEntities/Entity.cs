namespace Core.Infrastructure.Domain.BaseEntities
{
    /// <summary>
    /// Represents the  entity
    /// </summary>

    public abstract class Entity<T> : IEntity<T>
    {
        #region Properties

        /// <summary>
        /// gets or sets Identifier of this Entity
        /// </summary>
        public virtual T Id { get; set; }

        #endregion

    }
}

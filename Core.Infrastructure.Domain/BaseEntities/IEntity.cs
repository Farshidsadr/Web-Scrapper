namespace Core.Infrastructure.Domain.BaseEntities
{
    /// <summary>
    /// Represents the entity
    /// </summary>

    public interface IEntity<T>
    {
        #region Properties

        /// <summary>
        /// gets or sets Identifier of this Entity
        /// </summary>
        T Id { get; set; }

        #endregion

    }
}

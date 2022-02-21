using System;

namespace Core.Infrastructure.Domain.BaseEntities
{
    /// <summary>
    /// Represents the base Entity
    /// </summary>
    public interface IBaseEntity<T> : IEntity<T>
    {
        #region Properties

        /// <summary>
        /// gets or sets date that this entity was created
        /// </summary>
        DateTime CreatedOn { get; set; }
        /// <summary>
        /// gets or sets Date that this entity was updated
        /// </summary>
        DateTime ModifiedOn { get; set; }

        #endregion

    }
}

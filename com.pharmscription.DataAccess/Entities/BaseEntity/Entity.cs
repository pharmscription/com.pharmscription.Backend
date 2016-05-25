using System;
using com.pharmscription.DataAccess.SharedInterfaces;
using com.pharmscription.Infrastructure.EntityHelper;

namespace com.pharmscription.DataAccess.Entities.BaseEntity
{
    public abstract class Entity : IEntity
    {
        #region Members

        private int? _requestedHashCode;
        private Guid _id;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the persistent object identifier
        /// </summary>
        public virtual Guid Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnIdChanged();
            }
        }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// When POID is changed
        /// </summary>
        protected virtual void OnIdChanged()
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Check if this entity is transient, ie, without identity at this moment
        /// </summary>
        /// <returns>True if entity is transient, else false</returns>
        public bool IsTransient()
        {
            return Id == Guid.Empty;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            return item.Id == Id;
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Equals(left, null))
                return (Equals(right, null));
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public void GenerateAndSetNewId()
        {
            Id = IdentityGenerator.NewSequentialGuid();
        }

        #endregion
    }
}

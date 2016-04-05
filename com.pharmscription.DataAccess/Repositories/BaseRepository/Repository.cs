using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Repositories.BaseRepository
{
    public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
    {

        #region Members

        readonly IQueryableUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="unitOfWork">Associated Unit NameOf Work</param>
        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        public IUnitOfWork UnitOfWork => _unitOfWork;

        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="IRepository{TEntity}"/></param>
        public virtual void Add(TEntity item)
        {

            if (item != null)
            {
                GetSet().Add(item);
            }
            else
            {
                throw new NullReferenceException("Item was null");
            }
        }
        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="IRepository{TEntity}"/></param>
        public virtual void Remove(TEntity item)
        {
            if (item != null)
            {
                //attach item if not exist
                _unitOfWork.Attach(item);

                //set as "removed"
                GetSet().Remove(item);
            }
            else
            {
                throw new NullReferenceException("Item was null");
            }
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="IRepository{TEntity}"/></param>
        public virtual void TrackItem(TEntity item)
        {
            if (item != null)
            {
                _unitOfWork.Attach(item);
            }
            else
            {
                throw new NullReferenceException("Item was null");
            }
                
        }

        public virtual void UntrackItem(TEntity item)
        {
            if (item != null)
            {
                _unitOfWork.Detach(item);
            }
            else
            {
                throw new NullReferenceException("Item was null");
            }
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <param name="item"><see cref="IRepository{TEntity}"/></param>
        public virtual void Modify(TEntity item)
        {
            if (item != null)
            {
                _unitOfWork.SetModified(item);
            }
            else
            {
                throw new NullReferenceException("Item was null");
            }
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <param name="id"><see cref="IRepository{TEntity}"/></param>
        /// <returns><see cref="IRepository{TEntity}"/></returns>
        public virtual TEntity Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                return GetSet().Find(id);
            }
            throw new NullReferenceException("Guid was empty");
        }

        public virtual Task<TEntity> GetAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                return GetSet().FirstAsync(e => e.Id == id);
            }
            return null;
        }
        public virtual IEnumerable<TEntity> Find(Guid id)
        {
            if (id != Guid.Empty)
            {
                return GetSet().Where(e => e.Id == id);
            }
            throw new NullReferenceException("Guid was empty");
            
        }
        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <returns><see cref="IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetSet().AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetAllAsNoTracking()
        {
            return GetSet().AsNoTracking().AsEnumerable();
        }

        public virtual int Count()
        {
            return GetSet().Count();
        }

        public virtual int Count(Func<TEntity, bool> predicate)
        {
            return GetSet().Count(predicate);
        }

        //TODO: Implement Specification-Pattern
        //public virtual IEnumerable<TEntity> AllMatching(Domain.Seedwork.Specification.ISpecification<TEntity> specification)
        //{
        //    return GetSet().Where(specification.SatisfiedBy())
        //                   .AsEnumerable();
        //}
        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <returns><see cref="IRepository{TEntity}"/></returns>
        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TKProperty"></typeparam>
        /// <param name="pageIndex"><see cref="IRepository{TEntity}"/></param>
        /// <param name="pageCount"><see cref="IRepository{TEntity}"/></param>
        /// <param name="orderByExpression"><see cref="IRepository{TEntity}"/></param>
        /// <param name="ascending"><see cref="IRepository{TEntity}"/></param>
        /// <returns><see cref="IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetPaged<TKProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending)
        {
            var set = GetSet();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsEnumerable();
            }
            return set.OrderByDescending(orderByExpression)
                .Skip(pageCount * pageIndex)
                .Take(pageCount)
                .AsEnumerable();
        }
        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <param name="filter"><see cref="IRepository{TEntity}"/></param>
        /// <returns><see cref="IRepository{TEntity}"/></returns>
        public virtual IEnumerable<TEntity> GetFiltered(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Where(filter)
                           .AsEnumerable();
        }


        /// <summary>
        /// <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <param name="persisted"><see cref="IRepository{TEntity}"/></param>
        /// <param name="current"><see cref="IRepository{TEntity}"/></param>
        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _unitOfWork.ApplyCurrentValues(persisted, current);
        }

        #endregion

        #region Private Methods

        protected IDbSet<TEntity> GetSet()
        {
            return _unitOfWork.CreateSet<TEntity>();
        }
        #endregion
    }
}
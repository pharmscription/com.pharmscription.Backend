﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace com.pharmscription.DataAccess.SharedInterfaces
{
   public interface IRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Gets the unit of work in this repository
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        TEntity Add(TEntity item);

        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Remove(TEntity item);

        /// <summary>
        /// Track entity into this repository, really in UnitOfWork. 
        /// In EF this can be done with Attach and with Update in NH
        /// </summary>
        /// <param name="item">Item to attach</param>
        void TrackItem(TEntity item);

        /// <summary>
        /// Track entity into this repository, really in UnitOfWork. 
        /// In EF this can be done with Attach and with Update in NH
        /// </summary>
        /// <param name="item">Item to attach</param>
        void UntrackItem(TEntity item);

        /// <summary>
        /// Set entity modified 
        /// </summary>
        /// <param name="item">Item to attach</param>
        void Modify(TEntity item);

        /// <summary>
        /// Sets modified entity into the repository. 
        /// When calling Commit() method in UnitOfWork 
        /// these changes will be saved into the storage
        /// </summary>
        /// <param name="persisted">The persisted item</param>
        /// <param name="current">The current item</param>
        void Merge(TEntity persisted, TEntity current);

        /// <summary>
        /// Get element by entity key
        /// </summary>
        /// <param name="id">Id of searched entity</param>
        /// <returns>Found entity</returns>
        TEntity Get(Guid id);

        /// <summary>
        /// Get element by entity key async
        /// </summary>
        /// <param name="id">Id of searched entity</param>
        /// <returns>Found Entity</returns>
        Task<TEntity> GetAsync(Guid id);

        /// <summary>
        /// Get element by entity key async, But throws NotFoundException when no element is Present
        /// </summary>
        /// <param name="id">Id of searched entity</param>
        /// <returns>Found entity</returns>
        Task<TEntity> GetAsyncOrThrow(Guid id);

        /// <summary>
        /// Checks if Entity exists, But throws NotFoundException when no element is Present
        /// </summary>
        /// <param name="id">Id of entity to be checked</param>
        /// <returns><see cref="Task"/> which returns a bool wheter entity exists or not</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "bool is no spelling mistake")]
        Task<bool> CheckIfEntityExists(Guid id);
        IEnumerable<TEntity> Find(Guid id);

        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllAsNoTracking();

        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetPaged<TKProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending);

        /// <summary>
        /// Get  elements of type {T} in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);

        int Count();
        int Count(Func<TEntity, bool> predicate);
    }
}

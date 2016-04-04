using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess.Tests.UnitOfWork
{
    class MockPharmscriptionUnitOfWork<TEntity>  : IPharmscriptionUnitOfWork where TEntity : class
    {
        private readonly List<TEntity> _attachedEntites;
        private readonly List<TEntity> _persistedEntites;

        public MockPharmscriptionUnitOfWork()
        {
            _attachedEntites = new List<TEntity>();
            _persistedEntites = new List<TEntity>();
        } 

        public void Dispose()
        {
            _attachedEntites.Clear();
            _persistedEntites.Clear();
        }

        public void Commit()
        {
            foreach (var entity in _attachedEntites)
            {
                _persistedEntites.Add(entity);
            }
            _attachedEntites.Clear();
        }

        public Task<int> CommitAsync()
        {
            foreach (var entity in _attachedEntites)
            {
                _persistedEntites.Add(entity);
            }
            _attachedEntites.Clear();
            return Task.Factory.StartNew(
                () => 1);
        }

        public void CommitAndRefreshChanges()
        {
            throw new NotImplementedException();
        }

        public void RollbackChanges()
        {
            _attachedEntites.Clear();
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Detach<TEntity>(TEntity item) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public IDbSet<Patient> Patients { get; }
    }
}

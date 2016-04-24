using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.DataAccess.Migrations;

namespace com.pharmscription.DataAccess.UnitOfWork
{
    public class PharmscriptionUnitOfWork : DbContext, IPharmscriptionUnitOfWork
    {
        static PharmscriptionUnitOfWork()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<PharmscriptionUnitOfWork, Configuration>());
            Database.SetInitializer(new DropCreateDatabaseAlways<PharmscriptionUnitOfWork>());
        }
        #region IPharmscriptionUnitOfWork Members

        private IDbSet<Patient> _patients;
        public virtual IDbSet<Patient> Patients => _patients ?? (_patients = base.Set<Patient>());

        private IDbSet<Drug> _drugs;
        public virtual IDbSet<Drug> Drugs => _drugs ?? (_drugs = base.Set<Drug>());

        private IDbSet<Prescription> _prescriptions;
        public virtual IDbSet<Prescription> Prescriptions
            => _prescriptions ?? (_prescriptions = base.Set<Prescription>());

        private IDbSet<CounterProposal> _counterProposals;

        public virtual IDbSet<CounterProposal> CounterProposals
            => _counterProposals ?? (_counterProposals
            = base.Set<CounterProposal>());

        private IDbSet<Dispense> _dispenses;
        public virtual IDbSet<Dispense> Dispenses
            => _dispenses ?? (_dispenses
            = base.Set<Dispense>());

        private IDbSet<DrugItem> _drugItems;
        public virtual IDbSet<DrugItem> DrugItems
            => _drugItems ?? (_drugItems
            = base.Set<DrugItem>());
        #endregion

        #region IQueryableUnitOfWork Members

        public virtual IDbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            //attach and set as unchanged
            Entry(item).State = EntityState.Unchanged;
        }

        public void Detach<TEntity>(TEntity item)
            where TEntity : class
        {
            Entry(item).State = EntityState.Detached;
        }

        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            //this operation also attach item in object state manager
            Entry(item).State = EntityState.Modified;
        }
        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if not is attached, attach original and set current values
            Entry(original).CurrentValues.SetValues(current);
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                var result = await SaveChangesAsync();
                return result;
            }
            catch (DbEntityValidationException ex)
            {
                // rethrow a more detailed exception explaining the problem based on EntityValidationErrors
                throw new Exception(string.Join(";", ex.EntityValidationErrors.SelectMany(evr => evr.ValidationErrors).Select(ve => ve.ErrorMessage)), ex);
            }
        }

        public void Commit()
        {
            SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));

                }
            } while (saveFailed);

        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        #endregion

        #region DbContext Overrides

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            var changeSet = ChangeTracker.Entries<Entity>();

            if (changeSet != null)
            {
                ManipulateIdCreatedAndModifiedDateBeforeCommit(changeSet);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync()
        {
            var changeSet = ChangeTracker.Entries<Entity>();

            if (changeSet != null)
            {
                ManipulateIdCreatedAndModifiedDateBeforeCommit(changeSet);
            }

            return base.SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            var changeSet = ChangeTracker.Entries<Entity>();

            if (changeSet != null)
            {
                ManipulateIdCreatedAndModifiedDateBeforeCommit(changeSet);
            }

            return base.SaveChanges();
        }

        private void ManipulateIdCreatedAndModifiedDateBeforeCommit(IEnumerable<DbEntityEntry<Entity>> changeSet)
        {
            var dbEntityEntries = changeSet as DbEntityEntry<Entity>[] ?? changeSet.ToArray();
            foreach (var entry in dbEntityEntries.Where(c => c.State == EntityState.Added))
            {
                entry.Entity.ModifiedDate = DateTime.Now;
                if (entry.Entity.CreatedDate == null)
                {
                    entry.Entity.CreatedDate = entry.Entity.ModifiedDate;
                }
                if (entry.Entity.Id == Guid.Empty)
                {
                    entry.Entity.GenerateAndSetNewId();
                }
            }
            foreach (var entry in dbEntityEntries.Where(c => c.State == EntityState.Modified))
            {
                entry.Entity.ModifiedDate = DateTime.Now;
            }
        }

        #endregion
    }
}

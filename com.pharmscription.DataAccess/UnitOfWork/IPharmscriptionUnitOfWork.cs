using System.Data.Entity;
using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.UnitOfWork
{
    using Entities.DrugPriceEntity;
    using Entities.DrugStoreEntity;

    public interface IPharmscriptionUnitOfWork : IQueryableUnitOfWork
    {
        IDbSet<Patient> Patients { get; }
        IDbSet<Drug> Drugs { get; }
        IDbSet<Prescription> Prescriptions { get; }
        IDbSet<CounterProposal> CounterProposals { get; }
        IDbSet<Dispense> Dispenses { get; }
        IDbSet<DrugItem> DrugItems { get; }
        IDbSet<DrugStore> DrugStores { get; }
        IDbSet<DrugPrice> DrugPrices { get; }
    }
}

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
    using com.pharmscription.DataAccess.Entities.DoctorEntity;
    using com.pharmscription.DataAccess.Entities.DrugistEntity;
    using com.pharmscription.DataAccess.Entities.DrugstoreEmployeeEntity;

    public interface IPharmscriptionUnitOfWork : IQueryableUnitOfWork
    {
        IDbSet<Patient> Patients { get; }
        IDbSet<Drug> Drugs { get; }
        IDbSet<Prescription> Prescriptions { get; }
        IDbSet<CounterProposal> CounterProposals { get; }
        IDbSet<Dispense> Dispenses { get; }
        IDbSet<DrugItem> DrugItems { get; }
        IDbSet<Doctor> Doctors { get; }
        IDbSet<Drugist> Drugists { get; }
        IDbSet<DrugstoreEmployee> DrugstoreEmployees { get; }
    }
}

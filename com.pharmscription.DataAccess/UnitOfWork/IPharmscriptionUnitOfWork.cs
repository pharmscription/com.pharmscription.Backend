using System.Data.Entity;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.UnitOfWork
{
    public interface IPharmscriptionUnitOfWork : IQueryableUnitOfWork
    {
        IDbSet<Patient> Patients { get; }
        IDbSet<Drug> Drugs { get; }
        IDbSet<Prescription> Prescriptions { get; }
    }
}

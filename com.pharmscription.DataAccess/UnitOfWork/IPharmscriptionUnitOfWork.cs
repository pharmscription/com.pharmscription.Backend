using System.Data.Entity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.UnitOfWork
{
    public interface IPharmscriptionUnitOfWork : IQueryableUnitOfWork
    {
        IDbSet<Patient> Patients { get; }
    }
}

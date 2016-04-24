using System;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    using System.Threading.Tasks;
    using com.pharmscription.DataAccess.SharedInterfaces;
    using com.pharmscription.DataAccess.Entities.PatientEntity;

    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient> GetByAhvNumber(string ahvNumber);
        bool Exists(string ahvNumber);
        Task<Patient> GetWithPrescriptions(Guid id);


    }
}

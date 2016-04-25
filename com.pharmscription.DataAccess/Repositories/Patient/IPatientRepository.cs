using System;
using System.Linq;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    using System.Threading.Tasks;
    using SharedInterfaces;
    using Entities.PatientEntity;

    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient> GetByAhvNumber(string ahvNumber);
        bool Exists(string ahvNumber);
        Task<Patient> GetWithPrescriptions(Guid id);
        IQueryable<Patient> GetWithAllNavs();

    }
}

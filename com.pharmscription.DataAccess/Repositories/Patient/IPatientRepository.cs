using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    public interface IPatientRepository : IRepository<Entities.PatientEntity.Patient>
    {
        Entities.PatientEntity.Patient GetByAhvNumber(string ahvNumber);
        bool Exists(string ahvNumber);
       
    }
}

using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.AccountEntity;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Security;

namespace com.pharmscription.BusinessLogic.Patient
{
    public interface IPatientManager
    {
        [Authorized(Role=Role.Doctor)]
        Task<PatientDto> Lookup(string ahvNumber);
        Task<PatientDto> Add(PatientDto patient);
        Task<PatientDto> Edit(PatientDto patient);
        Task<PatientDto> Find(string ahvNumber);
        Task<PatientDto> GetById(string id);
        Task<PatientDto> RemoveById(string id);
    }
}


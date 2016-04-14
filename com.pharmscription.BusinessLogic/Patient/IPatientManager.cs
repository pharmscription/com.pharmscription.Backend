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
        [Authorized(Role = Role.Doctor)]
        Task<PatientDto> Add(PatientDto patient);
        [Authorized(Role = Role.Doctor)]
        [Authorized(Role = Role.Patient)]
        Task<PatientDto> Edit(PatientDto patient);
        [Authorized(Role = Role.Doctor)]
        Task<PatientDto> Find(string ahvNumber);
        [Authorized(Role = Role.Doctor)]
        Task<PatientDto> GetById(string id);
        Task<PatientDto> RemoveById(string id);
    }
}


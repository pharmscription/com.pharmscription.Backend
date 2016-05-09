using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Patient
{
    public interface IPatientManager
    {
        Task<PatientDto> Lookup(string ahvNumber);
        Task<PatientDto> Add(PatientDto patient);
        Task<PatientDto> Find(string ahvNumber);
        Task<PatientDto> GetById(string id);
        Task<PatientDto> Update(PatientDto patient);
    }
}

